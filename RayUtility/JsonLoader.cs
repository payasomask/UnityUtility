using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class JsonLoader {
  public static JsonLoader _JsonLoader = null;

  //readonly string cardJsonFileName = "chara_cards.feats.obj";
  //readonly string featJsonFileName = "feats.obj";
  //readonly string passiveSkillJsonFileName = "passive_skills.obj";
  //readonly string actionCardJsonFileName = "action_cards.obj";
  //readonly string eventCardJsonFileName = "event_cards.obj";
  //readonly string avatarItemsJsonFileName = "avatar_items.obj";
  //readonly string treasureCardJsonFileName = "treasure_datas.obj";
  readonly string JsonString = "JsonString";


  Dictionary<string, CharacterCardJson> characterCardsDic = new Dictionary<string, CharacterCardJson>();
  Dictionary<string, FeatJson> featDic = new Dictionary<string, FeatJson>();
  Dictionary<string, PassiveJson> passiveDic = new Dictionary<string, PassiveJson>();
  Dictionary<string, ActionCardJson> actionCardsDic = new Dictionary<string, ActionCardJson>();
  Dictionary<string, EventCardJson> eventCardsDic = new Dictionary<string, EventCardJson>();
  Dictionary<string, ItemCardJson> ItemCardsDic = new Dictionary<string, ItemCardJson>();
  Dictionary<string, treasureCardJson> treasuresDic = new Dictionary<string, treasureCardJson>();
  Dictionary<string, string> stringDic = new Dictionary<string, string>();

  Dictionary<string, string> phaseConvertDic = new Dictionary<string, string>() { { "攻撃", "ATK" }, { "防御", "DEF" }, { "移動", "MOV" } };
  Dictionary<string, string> distanceConvertDic = new Dictionary<string, string>() { { "近", "S" }, { "中", "M" }, { "遠", "L" } };
  Dictionary<string, string> requirementConvertDic = new Dictionary<string, string>() { { "移", "M" }, { "近", "S" }, { "遠", "A" }, { "特", "E" }, { "防", "D" }, { "無", "W" }, { "[SAD]", "[SAD]" } };

  public void Init() {
    // start init

    // parse feat
    //Dictionary<string, object> feats = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(featJsonFileName));
    //featDic = new Dictionary<string, FeatJson>(feats.Count);
    //foreach (var feat in feats) {
    //  Dictionary<string, object> featData = (Dictionary<string, object>)feat.Value;
    //  FeatJson f = new FeatJson();
    //  f.caption = (string)featData[FeatTableMapp.caption.ToString()];
    //  f.condition = (string)featData[FeatTableMapp.condition.ToString()];
    //  f.dice_attribute = (string)featData[FeatTableMapp.dice_attribute.ToString()];
    //  f.effect_image = (string)featData[FeatTableMapp.effect_image.ToString()];
    //  f.feat_no = (int)(System.Int64)featData[FeatTableMapp.feat_no.ToString()];
    //  f.id = (int)(System.Int64)featData[FeatTableMapp.id.ToString()];
    //  f.name = (string)featData[FeatTableMapp.name.ToString()];
    //  f.pow = (int)(System.Int64)featData[FeatTableMapp.pow.ToString()];
    //  featDic.Add(f.id.ToString(), f);
    //}

    //// parse passvie
    //Dictionary<string, object> passives = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(passiveSkillJsonFileName));
    //passiveDic = new Dictionary<string, PassiveJson>(passives.Count);
    //foreach (var passive in passives) {
    //  Dictionary<string, object> passiveData = (Dictionary<string, object>)passive.Value;
    //  PassiveJson p = new PassiveJson();
    //  p.caption = (string)passiveData[PassiveTableMapp.caption.ToString()];
    //  p.effect_image = (string)passiveData[PassiveTableMapp.effect_image.ToString()];
    //  p.id = (int)(System.Int64)passiveData[PassiveTableMapp.id.ToString()];
    //  p.name = (string)passiveData[PassiveTableMapp.name.ToString()];
    //  p.pow = (int)(System.Int64)passiveData[PassiveTableMapp.pow.ToString()];
    //  p.passive_skill_no = (int)(System.Int64)passiveData[PassiveTableMapp.passive_skill_no.ToString()];
    //  passiveDic.Add(p.id.ToString(), p);
    //}

    //// parse carddata
    //Dictionary<string, object> characterCards = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(cardJsonFileName));
    //characterCardsDic = new Dictionary<string, CharacterCardJson>(characterCards.Count);
    //foreach (var cardDatas in characterCards) {
    //  Dictionary<string, object> cardData = (Dictionary<string, object>)cardDatas.Value;

    //  List<object> cardfeats = (List<object>)cardData[CardTableMapp.feats.ToString()];
    //  List<Feat> tmpfeats = new List<Feat>(cardfeats.Count);
    //  for (int i = 0; i < cardfeats.Count; i++) {
    //    int featid = (int)(System.Int64)cardfeats[i];
    //    tmpfeats.Add(convertFeatCaption(getFeatJson(featid.ToString()).name, featid, getFeatJson(featid.ToString()).caption));
    //  }

    //  List<object> cardpassives = (List<object>)cardData[CardTableMapp.passive_skills.ToString()];
    //  List<PassiveJson> tmppassives = new List<PassiveJson>(cardpassives.Count);
    //  for (int i = 0; i < cardpassives.Count; i++) {
    //    int passiveId = (int)(System.Int64)cardpassives[i];
    //    tmppassives.Add(getPassiveJson(passiveId.ToString()));
    //  }

    //  CharacterCardJson cc = new CharacterCardJson();
    //  cc.feats = tmpfeats.ToArray();
    //  cc.passive_skills = tmppassives.ToArray();
    //  cc.charactor_id = (int)(System.Int64)cardData[CardTableMapp.charactor_id.ToString()];
    //  cc.ab_name = (string)cardData[CardTableMapp.ab_name.ToString()];
    //  cc.ap = (int)(System.Int64)cardData[CardTableMapp.ap.ToString()];
    //  cc.artifact_image = (string)cardData[CardTableMapp.artifact_image.ToString()];
    //  cc.bg_image = (string)cardData[CardTableMapp.bg_image.ToString()];
    //  cc.caption = (string)cardData[CardTableMapp.caption.ToString()];
    //  cc.chara_image = (string)cardData[CardTableMapp.chara_image.ToString()];
    //  cc.dp = (int)(System.Int64)cardData[CardTableMapp.dp.ToString()];
    //  cc.deck_cost = (int)(System.Int64)cardData[CardTableMapp.deck_cost.ToString()];
    //  cc.hp = (int)(System.Int64)cardData[CardTableMapp.hp.ToString()];
    //  cc.id = (int)(System.Int64)cardData[CardTableMapp.id.ToString()];
    //  cc.kind = (int)(System.Int64)cardData[CardTableMapp.kind.ToString()];
    //  cc.level = (int)(System.Int64)cardData[CardTableMapp.level.ToString()];
    //  cc.name = (string)cardData[CardTableMapp.name.ToString()];
    //  cc.next_id = cardData[CardTableMapp.next_id.ToString()] == null ? -1 : (int)(System.Int64)cardData[CardTableMapp.next_id.ToString()];
    //  cc.rarity = (int)(System.Int64)cardData[CardTableMapp.rarity.ToString()];
    //  cc.slot = (int)(System.Int64)cardData[CardTableMapp.slot.ToString()];
    //  cc.stand_image = (string)cardData[CardTableMapp.stand_image.ToString()];
    //  characterCardsDic.Add(cc.id.ToString(), cc);
    //}

    //// parse actioncard
    //Dictionary<string, object> actionCards = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(actionCardJsonFileName));
    //actionCardsDic = new Dictionary<string, ActionCardJson>(actionCards.Count);
    //foreach (var actioncard in actionCards) {
    //  Dictionary<string, object> actioncardData = (Dictionary<string, object>)actioncard.Value;
    //  ActionCardJson ac = new ActionCardJson();
    //  ac.b_type = (int)(System.Int64)actioncardData[ActionCardMapp.b_type.ToString()];
    //  ac.b_value = (int)(System.Int64)actioncardData[ActionCardMapp.b_value.ToString()];
    //  ac.caption = (string)actioncardData[ActionCardMapp.caption.ToString()];
    //  ac.event_no = (int)(System.Int64)actioncardData[ActionCardMapp.event_no.ToString()];
    //  ac.id = (int)(System.Int64)actioncardData[ActionCardMapp.id.ToString()];
    //  ac.image = (string)actioncardData[ActionCardMapp.image.ToString()];
    //  ac.u_type = (int)(System.Int64)actioncardData[ActionCardMapp.u_type.ToString()];
    //  ac.u_value = (int)(System.Int64)actioncardData[ActionCardMapp.u_value.ToString()];
    //  ac.key = (string)actioncardData[ActionCardMapp.key.ToString()];
    //  ac.reverse = actioncardData.ContainsKey(ActionCardMapp.rev.ToString()) ? (bool)actioncardData[ActionCardMapp.rev.ToString()] : false;
    //  actionCardsDic.Add(actioncard.Key, ac);
    //}

    //// parse eventcard
    //Dictionary<string, object> eventCards = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(eventCardJsonFileName));
    //eventCardsDic = new Dictionary<string, EventCardJson>(eventCards.Count);
    //foreach (var eventcard in eventCards) {
    //  Dictionary<string, object> eventcardData = (Dictionary<string, object>)eventcard.Value;
    //  EventCardJson ec = new EventCardJson();
    //  ec.caption = (string)eventcardData[EventCardMapp.caption.ToString()];
    //  ec.event_no = (int)(System.Int64)eventcardData[EventCardMapp.event_no.ToString()];
    //  ec.id = (int)(System.Int64)eventcardData[EventCardMapp.id.ToString()];
    //  ec.image = (string)eventcardData[EventCardMapp.image.ToString()];
    //  ec.card_cost = (int)(System.Int64)eventcardData[EventCardMapp.card_cost.ToString()];
    //  ec.color = (int)(System.Int64)eventcardData[EventCardMapp.color.ToString()];
    //  ec.filler = (bool)eventcardData[EventCardMapp.filler.ToString()];
    //  ec.max_in_deck = (int)(System.Int64)eventcardData[EventCardMapp.max_in_deck.ToString()];
    //  ec.name = (string)eventcardData[EventCardMapp.name.ToString()];
    //  ec.restriction = (string)eventcardData[EventCardMapp.restriction.ToString()];
    //  eventCardsDic.Add(ec.id.ToString(), ec);
    //}

    //// parse avatar_Items
    //Dictionary<string, object> ItemCards = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(avatarItemsJsonFileName));
    //ItemCardsDic = new Dictionary<string, ItemCardJson>(ItemCards.Count);
    //foreach (var ItemCard in ItemCards) {
    //  Dictionary<string, object> ItemCardsData = (Dictionary<string, object>)ItemCard.Value;
    //  ItemCardJson ic = new ItemCardJson();
    //  ic.name = (string)ItemCardsData[ItemCardMapp.name.ToString()];
    //  ic.id = (int)(System.Int64)ItemCardsData[ItemCardMapp.id.ToString()];
    //  ic.image = (string)ItemCardsData[ItemCardMapp.image.ToString()];
    //  ic.cond = (string)ItemCardsData[ItemCardMapp.cond.ToString()];
    //  ic.duration = (int)(System.Int64)ItemCardsData[ItemCardMapp.duration.ToString()];
    //  ic.effect_image = (string)ItemCardsData[ItemCardMapp.effect_image.ToString()];
    //  ic.image_frame = (int)(System.Int64)ItemCardsData[ItemCardMapp.image_frame.ToString()];
    //  ic.item_no = (int)(System.Int64)ItemCardsData[ItemCardMapp.item_no.ToString()];
    //  ic.kind = (int)(System.Int64)ItemCardsData[ItemCardMapp.kind.ToString()];
    //  ic.sub_kind = (string)ItemCardsData[ItemCardMapp.sub_kind.ToString()];
    //  ic.caption = (string)ItemCardsData[ItemCardMapp.caption.ToString()];
    //  ItemCardsDic.Add(ic.id.ToString(), ic);
    //}

    //// parse treasures
    //Dictionary<string, object> treasureCards = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(treasureCardJsonFileName));
    //treasuresDic = new Dictionary<string, treasureCardJson>(treasureCards.Count);
    //foreach (var treasureCard in treasureCards) {
    //  Dictionary<string, object> treasureCardData = (Dictionary<string, object>)treasureCard.Value;
    //  treasureCardJson tc = new treasureCardJson();
    //  tc.name = (string)treasureCardData[treasureCardMapp.name.ToString()];
    //  tc.id = (int)(System.Int64)treasureCardData[treasureCardMapp.id.ToString()];
    //  tc.slot_type = (int)(System.Int64)treasureCardData[treasureCardMapp.slot_type.ToString()];
    //  tc.allocation_id = (string)treasureCardData[treasureCardMapp.allocation_id.ToString()];
    //  tc.allocation_type = (int)(System.Int64)treasureCardData[treasureCardMapp.allocation_type.ToString()];
    //  tc.treasure_type = (int)(System.Int64)treasureCardData[treasureCardMapp.treasure_type.ToString()];
    //  tc.value = (int)(System.Int64)treasureCardData[treasureCardMapp.value.ToString()];
    //  treasuresDic.Add(tc.id.ToString(), tc);
    //}

    //string Json
    Dictionary<string, object> stringtable = (Dictionary<string, object>)MiniJSON.Json.Deserialize(getJson(JsonString));
    foreach(var v in stringtable) {
      string lan = v.Key.Replace("String", "");
      Dictionary<string, object> lanStrings = (Dictionary<string, object>)v.Value;
      foreach(var vv in lanStrings) {
        string stringKey = vv.Key;
        stringDic.Add(stringKey, (string)vv.Value);
      }
    }

    Debug.Log("562 -Json Loaded");
  }

  public string getJson(string filename) {
    string json_text;
#if UNITY_EDITOR || UNITY_WEBGL
    TextAsset t = (TextAsset)Resources.Load(filename);
    json_text = t.text;
#elif UNITY_ANDROID && !UNITY_EDITOR
    TextAsset t = (TextAsset)Resources.Load(filename);
    json_text = t.text;
    //string file_path = Path.Combine(Application.streamingAssetsPath, "filename" + ".json");
    //return File.ReadAllText(file_path);
#endif

    return json_text;
  }

  public static float toFloat(object val) {
    float ret = 0;
    if (val.GetType() == typeof(double)) {
      ret = (float)(double)val;
    } else
    if (val.GetType() == typeof(float)) {
      ret = (float)val;
    } else
    if (val.GetType() == typeof(int)) {
      ret = (float)(int)val;
    } else
    if (val.GetType() == typeof(uint)) {
      ret = (float)(uint)val;
    } else
    if (val.GetType() == typeof(System.Int64)) {
      ret = (float)(System.Int64)val;
    } else
    if (val.GetType() == typeof(System.UInt64)) {
      ret = (float)(System.UInt64)val;
    }

    return ret;
  }

  public FeatJson getFeatJson(string id) {
    return featDic[id];
  }

  public PassiveJson getPassiveJson(string id) {
    return passiveDic[id];
  }

  public ItemCardJson getItemCardJson(string id) {
    return ItemCardsDic[id];
  }

  public treasureCardJson getTreasureCardJson(string id) {
    return treasuresDic[id];
  }

  public string getString(string id) {
    return stringDic[id];
  }

  Feat convertFeatCaption(string name, int id, string caption) {
    Regex rx = new Regex(@"\[(.+):(.+):(.+)\](.+)");
    Match matches = rx.Match(caption);

    Regex rx2 = new Regex(@"\d+");
    Feat f = new Feat();
    string phase = matches.Groups[1].ToString();
    phase = phaseConvertDic.ContainsKey(phase) ? phaseConvertDic[phase] : "";
    string[] distances = matches.Groups[2].ToString().Split(',');
    for (int i = 0; i < distances.Length; i++) {
      distances[i] = distanceConvertDic.ContainsKey(distances[i]) ? distanceConvertDic[distances[i]] : "";
    }
    string[] requirement = matches.Groups[3].ToString().Split(',');
    List<FesstRequirement> tmpFr = new List<FesstRequirement>(requirement.Length);
    foreach (var r in requirement) {
      FesstRequirement fr = new FesstRequirement();
      char[] cs = r.ToCharArray();

      // [攻撃: 近:] 這種?
      if (cs.Length == 0) {
        fr.type = "";
        fr.amount = "";
        fr.condition = "";
        tmpFr.Add(fr);
        continue;
      }

      string[] rr = Regex.Split(r, @"\d+");
      string condition = cs[r.Length - 1].ToString();
      string number = "-1";

      if (condition == "+" || condition == "-") {
        // 特3+
        number = cs[r.Length - 2].ToString();
        fr.type = requirementConvertDic.ContainsKey(rr[0]) ? requirementConvertDic[rr[0]] : "";
        fr.amount = number.ToString();
        fr.condition = condition;
      } else {
        // 移0
        number = condition;
        fr.type = requirementConvertDic.ContainsKey(rr[0]) ? requirementConvertDic[rr[0]] : "";
        fr.amount = number.ToString();
        fr.condition = "";
      }
      tmpFr.Add(fr);
    }

    f.id = id;
    f.name = name;
    f.phase = phase;
    f.distances = distances;
    f.requirements = tmpFr.ToArray();
    f.description = matches.Groups[matches.Groups.Count - 1].ToString().Replace("|", "\n");
    return f;
  }

  public CharacterCardJson getCharacterCard(string id) {
    if(id == "unknown") {
      return new CharacterCardJson() { id = -1, name = "unknown",ap = -1,dp = -1, hp = -1,level = -1 };
    }
    return characterCardsDic[id];
  }
  public ActionCardJson getActionCard(string id) {
    if (!actionCardsDic.ContainsKey(id))
      Debug.Log(id);
    return actionCardsDic[id];
  }
}

enum CardTableMapp {
  ab_name,
  ap,
  artifact_image,
  bg_image,
  caption,
  chara_image,
  charactor_id,
  deck_cost,
  dp,
  feats,
  hp,
  id,
  kind,
  level,
  name,
  next_id,
  passive_skills,
  rarity,
  slot,
  stand_image
}
enum FeatTableMapp {
  id,
  name,
  feat_no,
  pow,
  dice_attribute,
  effect_image,
  caption,
  condition,
}
enum PassiveTableMapp {
  id,
  name,
  passive_skill_no,
  pow,
  effect_image,
  caption
}

enum ActionCardMapp {
  b_type,
  b_value,
  caption,
  event_no,
  id,
  image,
  u_type,
  u_value,
  key,
  rev
}
enum EventCardMapp {
  id,
  name,
  event_no,
  card_cost,
  color,
  max_in_deck,
  restriction,
  image,
  caption,
  filler,
}


public class ActionCardJson {
  public int id;
  public int b_type;
  public int b_value;
  public string caption;
  public int event_no;
  public string image;
  public int u_type;
  public int u_value;
  public string key;
  public bool reverse;
}

enum ItemCardMapp {
  id,
  name,
  item_no,
  kind,
  sub_kind,
  duration,
  cond,
  image,
  image_frame,
  effect_image,
  caption
}

public class ItemCardJson {
  public int id;
  public string name;
  public int item_no;
  public int kind;
  public string sub_kind;
  public int duration;
  public string cond;
  public string image;
  public int image_frame;
  public string effect_image;
  public string caption;
}

enum treasureCardMapp {
id,
name,
allocation_type,
allocation_id,
treasure_type,
slot_type,
value,
}

public class treasureCardJson {
  public int id;
  public string name;
  public int allocation_type;
  public string allocation_id;
  public int treasure_type;
  public int slot_type;
	public int value;
}


public class EventCardJson {
  public int id;
  public string name;
  public int event_no;
  public int card_cost;
  public int color;
  public int max_in_deck;
  public string restriction;
  public string image;
  public string caption;
  public bool filler;
}

public class CharacterCardJson {
  public string ab_name;
  public int ap;
  public string artifact_image;
  public string bg_image;
  public string caption;
  public string chara_image;
  public int charactor_id;
  public int deck_cost;
  public int dp;
  public Feat[] feats;
  public int hp;
  public int id;
  public int kind;
  public int level;
  public string name;
  public int next_id;// if null, next_id = -1
  public PassiveJson[] passive_skills;
  public int rarity;
  public int slot;
  public string stand_image;
}

public class FeatJson {
  public int id;
  public string name;
  public int feat_no;
  public int pow;
  public string dice_attribute;
  public string effect_image;
  public string caption;
  public string condition;
}

public class Feat {
  public int id;
  public string name;
  public string phase;
  public string[] distances;
  public FesstRequirement[] requirements;
  public string description;
}

public class FesstRequirement {
  public string type;
  public string amount;
  public string condition;// + = -
  public string spriteName() {
    return "feat_" + type + amount + condition;
  }
}

public class PassiveJson {
  public int id;
  public string name;
  public int passive_skill_no;
  public int pow;
  public string effect_image;
  public string caption;
}



