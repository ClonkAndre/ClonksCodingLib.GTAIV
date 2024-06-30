using System;
using System.Collections.Generic;

using IVSDKDotNet;

// Version 0.1 by ItsClonkAndre

// TODO: This sadly doesn't work as how i first expected it to work...

namespace CCL.GTAIV
{
    /// <summary>
    /// Helper class which lets you save custom data to the current save file using unused global variables within the <see cref="IVTheScripts.GlobalVariables"/> array.
    /// 
    /// <para><b>Some examples:</b></para>
    /// <para>
    /// 
    /// <b>Saving data to the current save file</b><br/>
    /// First you need to add a key to the save file using this function: <see cref="ExtendedIVSave.AddKey(string)"/>.<br/>
    /// The "key" parameter of this function should be the name of your script for example "TestScript".<br/>
    /// The function returns <see langword="true"/> if the key got added sucessfully, and <see langword="false"/> if it was unable to add for example if the key already exists.
    /// 
    /// <br/><br/>
    /// Now that you added a key to the save file, you add a custom value of any type to it using the <see cref="ExtendedIVSave.SetValueForKey(string, object)"/> function.<br/>
    /// The "key" parameter of this function will be the same as your current script ("TestScript" in this example), and the "value" parameter will be the custom data you want to store.<br/>
    /// For example, if you want to save a variable of type <see cref="string"/> (string testVariable = "Test string";) you give the <see cref="ExtendedIVSave.SetValueForKey(string, object)"/> function your "testVariable" variable.
    /// 
    /// </para>
    /// <para>
    /// 
    /// <b>Reading data from the current save file</b><br/>
    /// To read data from the save file use this function: <see cref="ExtendedIVSave.GetValueFromKey{T}(string)"/>.<br/>
    /// The "key" parameter of this function should be the name of your script ("TestScript" in this example).<br/>
    /// The "<typeparamref name="T"/>" generic type will be the type of the data you saved. For example, if we want to retrieve the "testVariable" we saved, we do this: ExtendedIVSave.GetValueFromKey&lt;<see cref="string"/>&gt;("TestScript");
    /// 
    /// </para>
    /// <para>
    /// 
    /// <b>Removing data from the current save file</b><br/>
    /// To remove data from the save file use this function: <see cref="ExtendedIVSave.RemoveKey(string)"/>.<br/>
    /// The "key" parameter of this function should be the name of your script ("TestScript" in this example).
    /// 
    /// </para>
    /// <para>
    /// 
    /// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -<br/>
    /// In this example we only stored a single <see cref="string"/> object in the save file.<br/>
    /// You could now store a <see cref="List{T}"/> using <see cref="ExtendedIVSave.SetValueForKey(string, object)"/> to store multiple stuff in one key.
    /// 
    /// </para>
    /// </summary>
    internal class ExtendedIVSave
    {

        /// <summary>
        /// Gets the raw string that is currently saved in the <see cref="IVTheScripts.GlobalVariables"/> array at the current index and converts it to a <see cref="Dictionary{TKey, TValue}"/> where all the custom save data is stored in.
        /// </summary>
        /// <returns>The <see cref="Dictionary{TKey, TValue}"/> where all the data is stored in.</returns>
        public static Dictionary<string, string> GetSaveDataDictionary()
        {
            string str = IVTheScripts.GetGlobalString(63000);

            if (string.IsNullOrWhiteSpace(str))
                return null;

            return Helper.ConvertJsonStringToObject<Dictionary<string, string>>(InternalHelper.DecompressString(str));
        }

        /// <summary>
        /// Checks if the current save file has any custom save data stored in it.
        /// </summary>
        /// <returns>True if there is custom data attached to the current save file. Otherwise, false. Add new data with the <see cref="AddKey(string)"/> function.</returns>
        public static bool DoesSaveDataExists()
        {
            string str = IVTheScripts.GetGlobalString(63000);

            if (string.IsNullOrWhiteSpace(str))
                return false;
            //if (!str.StartsWith("{") && !str.EndsWith("}"))
            //    return false;

            return true;
        }
        /// <summary>
        /// Checks if a key exists in the data dictionary saved within the current save file.
        /// </summary>
        /// <param name="key">The name of the key.</param>
        /// <returns>True if the key exists in the data dictionary saved within the current save file. Otherwise, false.</returns>
        public static bool DoesKeyExists(string key)
        {
            if (!DoesSaveDataExists())
                return false;

            Dictionary<string, string> data = GetSaveDataDictionary();
        
            if (data == null)
                return false;

            return data.ContainsKey(key);
        }

        /// <summary>
        /// Adds a new key to the data dictionary saved within the current save file.
        /// </summary>
        /// <param name="key">The name of the key to add.</param>
        /// <returns>True if the <paramref name="key"/> was added in the data dictionary saved within the current save file. Otherwise, false.</returns>
        public static bool AddKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            // If there is no save data then create new
            if (!DoesSaveDataExists())
            {
                Dictionary<string, string> data = new Dictionary<string, string>();

                // Add key to data dictionary
                data.Add(key, "");

                // Update the save with the new data
                return UpdateSave(data);
            }

            // Append to current save data
            {
                Dictionary<string, string> data = GetSaveDataDictionary();

                if (data == null)
                    return false;

                // Check if this key already exists in data dictionary
                if (data.ContainsKey(key))
                    return false;

                // Add key to data dictionary
                data.Add(key, "");

                // Update the save with the new data
                return UpdateSave(data);
            }
        }
        /// <summary>
        /// Removes a key from the data dictionary saved within the current save file.
        /// </summary>
        /// <param name="key">The name of the key to remove.</param>
        /// <returns>True if the key was removed from the data dictionary saved within the current save file. Otherwise, false.</returns>
        public static bool RemoveKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;
            if (!DoesSaveDataExists())
                return false;

            Dictionary<string, string> data = GetSaveDataDictionary();

            if (data == null)
                return false;

            // Check if key exists in the data dictionary
            if (!data.ContainsKey(key))
                return false;

            // Remove the key from the data dictionary
            bool result = data.Remove(key);

            if (!result)
                return false;

            // Update the save with the new data
            return UpdateSave(data);
        }

        /// <summary>
        /// Gets the value from a key within the current save file.
        /// </summary>
        /// <typeparam name="T">The type of the value that is stored in the current save file that you want to retrieve.</typeparam>
        /// <param name="key">The name of the key to get the value from.</param>
        /// <returns>The value of type <typeparamref name="T"/> stored within the <paramref name="key"/>.</returns>
        public static T GetValueFromKey<T>(string key)
        {
            if (!DoesSaveDataExists())
                return default;

            Dictionary<string, string> data = GetSaveDataDictionary();

            if (data == null)
                return default;
            if (!data.ContainsKey(key))
                return default;

            // Get the raw json string for the value
            string str = data[key];

            if (string.IsNullOrWhiteSpace(str))
                return default;

            // Convert the json string to the target type
            return Helper.ConvertJsonStringToObject<T>(str);
        }
        /// <summary>
        /// Sets a value of a key within the current save file.
        /// </summary>
        /// <param name="key">The name of the key to store the value in.</param>
        /// <param name="value">The object to store within that key.</param>
        /// <returns>True if the value was set within the <paramref name="key"/>. Otherwise, false.</returns>
        public static bool SetValueForKey(string key, object value)
        {
            if (!DoesSaveDataExists())
                return false;
            if (!DoesKeyExists(key))
                return false;

            Dictionary<string, string> data = GetSaveDataDictionary();

            if (data == null)
                return false;

            // Set new value
            data[key] = value == null ? "" : Helper.ConvertObjectToJsonString(value, false);

            // Update the save with the new data
            return UpdateSave(data);
        }

        // - - - PRIVATE STUFF - - -

        // Updates the string that is saved within the GlobalVariables array for the current save file
        private static bool UpdateSave(Dictionary<string, string> newData)
        {
            if (newData == null)
                return false;

            // Unused globals up to: g_U63110 (Might even go WAY higher)

            // Convert the object to a json string
            string rawString = Helper.ConvertObjectToJsonString(newData, false);

            IVTheScripts.SetGlobal(63000, InternalHelper.CompressString(rawString));

            //IVGame.Console.PrintWarning(string.Format("rawString length: {0}", rawString.Length));
            //IVGame.Console.PrintWarning(string.Format("compressed rawString length: {0}", comp.CompressString(rawString).Length));

            //// Split up the string if it has a length of 264607 or higher
            //List<string> list = new List<string>();

            //if (rawString.Length > 264607)
            //{
            //    // Need to split up the string
            //    string strToAdd = rawString.Substring(264607);
            //    list.Add(strToAdd);

            //    while (strToAdd.Length > 264607)
            //    {
            //        strToAdd = strToAdd.Substring(0, 264607);
            //        list.Add(strToAdd);
            //    }

            //    IVGame.Console.PrintWarning("Chars left: " + strToAdd.Length.ToString());

            //    if (strToAdd.Length <= 264607)
            //        list.Add(strToAdd);
            //}
            //else
            //{
            //    list.Add(rawString);
            //}

            //for (int i = 0; i < list.Count; i++)
            //{
            //    int globalVariableIndex = 63000 + i;

            //    IVGame.Console.PrintWarning(string.Format("{0} got saved at {1}", i, globalVariableIndex));

            //    IVTheScripts.SetGlobal(globalVariableIndex, list[i]);
            //}

            return true;
        }

    }
}
