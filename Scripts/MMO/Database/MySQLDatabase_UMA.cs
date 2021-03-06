﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;

namespace MultiplayerARPG.MMO
{
    public partial class MySQLDatabase
    {
        [DevExtMethods("CreateCharacter")]
        public void CreateCharacter_UMA(string userId, IPlayerCharacterData characterData)
        {
            // Save uma data
            IList<byte> bytes = characterData.UmaAvatarData.GetBytes();
            string saveData = string.Empty;
            for (int i = 0; i < bytes.Count; ++i)
            {
                if (i > 0)
                    saveData += ",";
                saveData += bytes[i];
            }
            ExecuteNonQuery("INSERT INTO characterumasaves (id, data) VALUES (@id, @data)",
                new MySqlParameter("@id", characterData.Id),
                new MySqlParameter("@data", saveData));
        }

        [DevExtMethods("ReadCharacter")]
        public void ReadCharacter_UMA(
            PlayerCharacterData characterData,
            bool withEquipWeapons,
            bool withAttributes,
            bool withSkills,
            bool withSkillUsages,
            bool withBuffs,
            bool withEquipItems,
            bool withNonEquipItems,
            bool withSummons,
            bool withHotkeys,
            bool withQuests)
        {
            // Read uma data
            MySQLRowsReader reader = ExecuteReader("SELECT data FROM characterumasaves WHERE id=@id",
                new MySqlParameter("@id", characterData.Id));
            if (reader.Read())
            {
                string data = reader.GetString("data");
                string[] splitedData = data.Split(',');
                List<byte> bytes = new List<byte>();
                foreach (string entry in splitedData)
                {
                    bytes.Add(byte.Parse(entry));
                }
                UmaAvatarData umaAvatarData = new UmaAvatarData();
                umaAvatarData.SetBytes(bytes);
                characterData.UmaAvatarData = umaAvatarData;
            }
        }

        [DevExtMethods("UpdateCharacter")]
        public void UpdateCharacter_UMA(IPlayerCharacterData characterData)
        {
            // Save uma data
            IList<byte> bytes = characterData.UmaAvatarData.GetBytes();
            string saveData = string.Empty;
            for (int i = 0; i < bytes.Count; ++i)
            {
                if (i > 0)
                    saveData += ",";
                saveData += bytes[i];
            }
            ExecuteNonQuery("UPDATE characterumasaves SET data=@data WHERE id=@id",
                new MySqlParameter("@id", characterData.Id),
                new MySqlParameter("@data", saveData));
        }

        [DevExtMethods("DeleteCharacter")]
        public void DeleteCharacter_UMA(string userId, string id)
        {
            // Delete uma data
            ExecuteNonQuery("DELETE FROM characterumasaves WHERE id=@id",
                new MySqlParameter("@id", id));
        }
    }
}
