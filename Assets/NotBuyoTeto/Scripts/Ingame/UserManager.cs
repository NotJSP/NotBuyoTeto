using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NCMB;
using NotBuyoTeto.Constants;
using NotBuyoTeto.Utility;

namespace NotBuyoTeto.Ingame {
    public static class UserManager {
        public static void FetchUsernames(string[] userIds, ASyncValue<Dictionary<string, string>, NCMBException> result) {
            var query = new NCMBQuery<NCMBObject>(NCMBClassName.Users);
            query.FindAsync((objList, e) => {
                if (e != null) {
                    Debug.LogError("Usersの取得に失敗: " + e.Message);
                    result.Exception = e;
                } else {
                    result.Value = objList.ToDictionary(o => o.ObjectId, o => Convert.ToString(o["name"]));
                }
            });
        }
    }
}