const fs = require("fs");
const path = require("path");

const repoRoot = path.resolve(__dirname, "..", "..");

const dataSets = [
  {
    name: "hero",
    schemaPath: "Assets/Data/Schema/hero.schema.json",
    dataPath: "Assets/Data/Heroes/hero.json"
  },
  {
    name: "skill",
    schemaPath: "Assets/Data/Schema/skill.schema.json",
    dataPath: "Assets/Data/Skills/skill.json"
  },
  {
    name: "item",
    schemaPath: "Assets/Data/Schema/item.schema.json",
    dataPath: "Assets/Data/Items/item.json"
  },
  {
    name: "buff",
    schemaPath: "Assets/Data/Schema/buff.schema.json",
    dataPath: "Assets/Data/Buffs/buff.json"
  },
  {
    name: "match",
    schemaPath: "Assets/Data/Schema/match.schema.json",
    dataPath: "Assets/Data/Matches/match.json"
  },
  {
    name: "map",
    schemaPath: "Assets/Data/Schema/map.schema.json",
    dataPath: "Assets/Data/Maps/map.json"
  },
  {
    name: "bot",
    schemaPath: "Assets/Data/Schema/bot.schema.json",
    dataPath: "Assets/Data/Bots/bot.json"
  }
];

function readJson(relativePath) {
  const absolutePath = path.join(repoRoot, relativePath);
  try {
    return JSON.parse(fs.readFileSync(absolutePath, "utf8"));
  } catch (error) {
    throw new Error(`${relativePath}: ${error.message}`);
  }
}

function resolveRef(root, ref) {
  if (!ref.startsWith("#/")) {
    throw new Error(`Only local schema refs are supported: ${ref}`);
  }

  return ref
    .slice(2)
    .split("/")
    .reduce((current, segment) => {
      const key = segment.replace(/~1/g, "/").replace(/~0/g, "~");
      return current[key];
    }, root);
}

function isType(value, expectedType) {
  if (expectedType === "array") return Array.isArray(value);
  if (expectedType === "integer") return Number.isInteger(value);
  if (expectedType === "number") return typeof value === "number" && Number.isFinite(value);
  if (expectedType === "object") return value !== null && typeof value === "object" && !Array.isArray(value);
  if (expectedType === "string") return typeof value === "string";
  if (expectedType === "boolean") return typeof value === "boolean";
  return true;
}

function validateSchema(schema, data, rootSchema, location) {
  const errors = [];

  if (!schema || typeof schema !== "object") {
    return errors;
  }

  if (schema.$ref) {
    return validateSchema(resolveRef(rootSchema, schema.$ref), data, rootSchema, location);
  }

  if (schema.allOf) {
    for (const subSchema of schema.allOf) {
      errors.push(...validateSchema(subSchema, data, rootSchema, location));
    }
  }

  if (schema.if && validateSchema(schema.if, data, rootSchema, location).length === 0 && schema.then) {
    errors.push(...validateSchema(schema.then, data, rootSchema, location));
  }

  if (schema.not && validateSchema(schema.not, data, rootSchema, location).length === 0) {
    errors.push(`${location}: matched forbidden schema`);
  }

  if (schema.type && !isType(data, schema.type)) {
    errors.push(`${location}: expected ${schema.type}`);
    return errors;
  }

  if (schema.const !== undefined && JSON.stringify(data) !== JSON.stringify(schema.const)) {
    errors.push(`${location}: expected const ${JSON.stringify(schema.const)}`);
  }

  if (schema.enum && !schema.enum.some((item) => JSON.stringify(item) === JSON.stringify(data))) {
    errors.push(`${location}: value is not in enum`);
  }

  if (typeof data === "string" && schema.pattern && !new RegExp(schema.pattern).test(data)) {
    errors.push(`${location}: pattern mismatch`);
  }

  if (typeof data === "number") {
    if (schema.minimum !== undefined && data < schema.minimum) {
      errors.push(`${location}: below minimum ${schema.minimum}`);
    }
    if (schema.exclusiveMinimum !== undefined && data <= schema.exclusiveMinimum) {
      errors.push(`${location}: must be greater than ${schema.exclusiveMinimum}`);
    }
    if (schema.maximum !== undefined && data > schema.maximum) {
      errors.push(`${location}: above maximum ${schema.maximum}`);
    }
  }

  if (Array.isArray(data)) {
    if (schema.minItems !== undefined && data.length < schema.minItems) {
      errors.push(`${location}: has fewer than ${schema.minItems} items`);
    }
    if (schema.maxItems !== undefined && data.length > schema.maxItems) {
      errors.push(`${location}: has more than ${schema.maxItems} items`);
    }
    if (schema.uniqueItems) {
      const seen = new Set();
      for (const item of data) {
        const key = JSON.stringify(item);
        if (seen.has(key)) {
          errors.push(`${location}: duplicate item ${key}`);
        }
        seen.add(key);
      }
    }
    if (schema.items) {
      data.forEach((item, index) => {
        errors.push(...validateSchema(schema.items, item, rootSchema, `${location}[${index}]`));
      });
    }
  }

  if (data !== null && typeof data === "object" && !Array.isArray(data)) {
    if (schema.required) {
      for (const key of schema.required) {
        if (!(key in data)) {
          errors.push(`${location}.${key}: missing required property`);
        }
      }
    }

    if (schema.additionalProperties === false && schema.properties) {
      for (const key of Object.keys(data)) {
        if (!(key in schema.properties)) {
          errors.push(`${location}.${key}: additional property`);
        }
      }
    }

    if (schema.properties) {
      for (const [key, subSchema] of Object.entries(schema.properties)) {
        if (key in data) {
          errors.push(...validateSchema(subSchema, data[key], rootSchema, `${location}.${key}`));
        }
      }
    }
  }

  return errors;
}

function collectIds(items) {
  return new Set(items.map((item) => item.id));
}

function requireId(errors, source, idSet, id, targetName) {
  if (!idSet.has(id)) {
    errors.push(`${source}: unknown ${targetName} id "${id}"`);
  }
}

function collectMapLaneNodeIds(map) {
  const idsByLane = new Map();
  for (const lane of map.lanes) {
    idsByLane.set(lane.laneType, new Set(lane.pathNodes.map((node) => node.id)));
  }
  return idsByLane;
}

function validateCrossReferences(dataByName) {
  const errors = [];

  const heroIds = collectIds(dataByName.hero.heroes);
  const skillIds = collectIds(dataByName.skill.skills);
  const itemIds = collectIds(dataByName.item.items);
  const buffIds = collectIds(dataByName.buff.buffs);

  for (const hero of dataByName.hero.heroes) {
    for (const [slot, skillId] of Object.entries(hero.skills)) {
      requireId(errors, `hero "${hero.id}" skills.${slot}`, skillIds, skillId, "skill");
    }
  }

  for (const skill of dataByName.skill.skills) {
    for (const [index, effect] of skill.effects.entries()) {
      if (effect.buffId) {
        requireId(errors, `skill "${skill.id}" effects[${index}].buffId`, buffIds, effect.buffId, "buff");
      }
    }
  }

  for (const bot of dataByName.bot.bots) {
    for (const heroId of bot.heroPool) {
      requireId(errors, `bot "${bot.id}" heroPool`, heroIds, heroId, "hero");
    }

    for (const itemId of bot.itemBuildPlan.preferredItemIds) {
      requireId(errors, `bot "${bot.id}" preferredItemIds`, itemIds, itemId, "item");
    }

    for (const itemId of bot.itemBuildPlan.fallbackItemIds || []) {
      requireId(errors, `bot "${bot.id}" fallbackItemIds`, itemIds, itemId, "item");
    }
  }

  for (const mapData of dataByName.map.maps) {
    const nodeIdsByLane = collectMapLaneNodeIds(mapData);
    for (const route of mapData.minionWaveRoutes) {
      const laneNodeIds = nodeIdsByLane.get(route.laneType);
      if (!laneNodeIds) {
        errors.push(`map "${mapData.id}" route "${route.laneType}": missing lane`);
        continue;
      }

      for (const nodeId of route.bluePathNodeIds) {
        requireId(errors, `map "${mapData.id}" ${route.laneType}.bluePathNodeIds`, laneNodeIds, nodeId, "path node");
      }

      for (const nodeId of route.redPathNodeIds) {
        requireId(errors, `map "${mapData.id}" ${route.laneType}.redPathNodeIds`, laneNodeIds, nodeId, "path node");
      }
    }
  }

  return errors;
}

function main() {
  const dataByName = {};
  const allErrors = [];

  for (const dataSet of dataSets) {
    let schema;
    let data;

    try {
      schema = readJson(dataSet.schemaPath);
      data = readJson(dataSet.dataPath);
    } catch (error) {
      allErrors.push(error.message);
      continue;
    }

    const schemaErrors = validateSchema(schema, data, schema, dataSet.dataPath);
    if (schemaErrors.length > 0) {
      allErrors.push(...schemaErrors);
    } else {
      console.log(`${dataSet.dataPath} OK`);
    }

    dataByName[dataSet.name] = data;
  }

  if (Object.keys(dataByName).length === dataSets.length) {
    allErrors.push(...validateCrossReferences(dataByName));
  }

  if (allErrors.length > 0) {
    console.error("Data validation failed:");
    for (const error of allErrors) {
      console.error(`- ${error}`);
    }
    process.exit(1);
  }

  console.log("Cross-file references OK");
  console.log("All data validation checks passed.");
}

main();
