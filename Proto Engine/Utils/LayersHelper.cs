using Microsoft.Xna.Framework;
using Proto_Engine.Editor.Data.MapComponents.Instances;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proto_Engine.Utils
{
    public static class LayersHelper
    {
        public static int GetGridValueAt(List<int> grid, int width, int x, int y, int fallback)
        {
            if (x > width -1 || x < 0)
            {
                return fallback;
            }
            else if (width * y + x < grid.Count && width * y + x >= 0)
            {
                return grid[width * y + x];
            }
            else
            {
                return fallback;
            }
        }
        private static List<int[]> rulesTypes;
        public static List<int[]> GetRules28()
        {
            if (rulesTypes == null)
            {
                #region Rule Types
                int[] type0 = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                int[] type1 = { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int[] type2 = { 1, 1, 1, 1, 1, 1, -1, 1, -1 };
                int[] type3 = { -1, 1, 1, 1, 1, 1, -1, 1, 1 };
                int[] type4 = { -1, 1, -1, 1, 1, 1, 1, 1, 1 };
                int[] type5 = { 1, 1, -1, 1, 1, 1, 1, 1, -1 };
                int[] type6 = { -1, 1, 1, 1, 1, 1, 1, 1, -1 };
                int[] type7 = { 1, 1, -1, 1, 1, 1, -1, 1, 1 };
                int[] type8 = { 1, 1, 1, 1, 1, 1, 1, 1, -1 };
                int[] type9 = { 1, 1, 1, 1, 1, 1, -1, 1, 1 };
                int[] type10 = { -1, 1, 1, 1, 1, 1, 1, 1, 1 };
                int[] type11 = { 1, 1, -1, 1, 1, 1, 1, 1, 1 };
                int[] type12 = { 0, 1, 0, 1, 1, 1, 0, 1, 0 };
                int[] type13 = { 0, 1, 0, 1, 1, -1, 0, 1, 0 };
                int[] type14 = { 0, 1, 0, -1, 1, 1, 0, 1, 0 };
                int[] type15 = { 0, -1, 0, 1, 1, 1, 0, 1, 0 };
                int[] type16 = { 0, 1, 0, 1, 1, 1, 0, -1, 0 };
                int[] type17 = { 0, -1, 0, 1, 1, 1, 0, -1, 0 };
                int[] type18 = { 0, 1, 0, -1, 1, -1, 0, 1, 0 };
                int[] type19 = { 0, -1, 0, -1, 1, -1, 0, 1, 0 };
                int[] type20 = { 0, 1, 0, -1, 1, -1, 0, -1, 0 };
                int[] type21 = { 0, -1, 0, -1, 1, 1, 0, -1, 0 };
                int[] type22 = { 0, -1, 0, 1, 1, -1, 0, -1, 0 };
                int[] type23 = { 0, -1, 0, -1, 1, -1, 0, -1, 0 };
                int[] type24 = { 0, -1, 0, -1, 1, 1, 0, 1, 0 };
                int[] type25 = { 0, -1, 0, 1, 1, -1, 0, 1, 0 };
                int[] type26 = { 0, 1, 0, -1, 1, 1, 0, -1, 0 };
                int[] type27 = { 0, 1, 0, 1, 1, -1, 0, -1, 0 };
                #endregion
                rulesTypes = new List<int[]>();
                #region Adds
                rulesTypes.Add(type27);
                rulesTypes.Add(type26);
                rulesTypes.Add(type25);
                rulesTypes.Add(type24);
                rulesTypes.Add(type23);
                rulesTypes.Add(type22);
                rulesTypes.Add(type21);
                rulesTypes.Add(type20);
                rulesTypes.Add(type19);
                rulesTypes.Add(type18);
                rulesTypes.Add(type17);
                rulesTypes.Add(type16);
                rulesTypes.Add(type15);
                rulesTypes.Add(type14);
                rulesTypes.Add(type13);
                rulesTypes.Add(type12);
                rulesTypes.Add(type11);
                rulesTypes.Add(type10);
                rulesTypes.Add(type9);
                rulesTypes.Add(type8);
                rulesTypes.Add(type7);
                rulesTypes.Add(type6);
                rulesTypes.Add(type5);
                rulesTypes.Add(type4);
                rulesTypes.Add(type3);
                rulesTypes.Add(type2);
                rulesTypes.Add(type1);
                rulesTypes.Add(type0);
                #endregion
                rulesTypes.Reverse();
            }
            return rulesTypes;
        }
        public static int[] GetAroundTile(List<int> grid, int width, int tileX, int tileY, int size, int fallback)
        {
            if (size % 2 == 0) throw new ArgumentException("The given size was not odd");
            int[] result = new int[size * size];
            int i = (size - 1) / 2;
            int counter = 0;
            for (int y = -i; y < i + 1; y++)
            {
                for (int x = -i; x < i + 1; x++)
                {
                    result[counter] = GetGridValueAt(grid, width, tileX + x, tileY + y, fallback);
                    counter++;
                }
            }
            return result;
        }
        public static int GetTileType(List<int> grid, int width, int tileX, int tileY)
        {
            List<int[]> rules = GetRules28();
            int ret = 0;
            foreach (int[] rule in rules)
            {
                bool IsItThisRule = true;
                
                // Determine the rule size
                int ruleSize;
                if (rule.Length > 9)
                    ruleSize = 5;
                else ruleSize = 3;

                int[] tileAndAround = GetAroundTile(grid, width, tileX, tileY, ruleSize, 1);
                for (int i = 0; i < rule.Length; i++)
                {
                    if (rule[i] == 0) continue;
                    else if (rule[i] == -1 && tileAndAround[i] == 1)
                    {
                        IsItThisRule = false;
                        break;
                    }
                    else if (rule[i] == 1 && tileAndAround[i] != 1)
                    {
                        IsItThisRule = false;
                        break;
                    }
                }
                if (IsItThisRule)
                {
                    return ret;
                }
                ret++;
            }
            return -1;
        }
    }
}
