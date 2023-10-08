﻿using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using EFT;
using HarmonyLib;
using Newtonsoft.Json;

namespace RaiRai.ColorConverter
{
    [BepInPlugin("com.rairai.colorconverterapi.eft", "ColorConverterAPI", "1.0.0")]
    public class Plugin: BaseUnityPlugin
    {
        public Plugin()
        {
            var EFTTypes = typeof(AbstractGame).Assembly.GetTypes();
            try
            {
                var convertersClass = EFTTypes
                    .Single(type => type.GetField("Converters", BindingFlags.Public | BindingFlags.Static) != null);
                var converters = Traverse.Create(convertersClass).Field<JsonConverter[]>("Converters").Value;

                var newConverters = converters.Prepend(new CustomColorConverter());
                Traverse.Create(convertersClass).Field("Converters").SetValue(newConverters.ToArray());
                new ColorConverterPatch().Enable();
            }
            catch (Exception ex)
            {
                Logger.LogError($"{GetType().Name}: {ex}");
                throw;
            }

        }
    }
}


/*
                              ,,,,,,                                                            ,,,,,,,,.                       
                            .,,,,,,,,,                                                   ,,,,,,,,,,,,,,,,                       
                           ,,,,,,,,,,,,,                                             ,,,,,,,,,,,,,,,,,,,,,                      
                          ,,,,,,,,,,,,,,,,                                        ,,,,,,,,,,,,,***,,,,,,,,                      
                         ,,,,,,,****,,,,,,,,         *********                  ,,,,,,,,,,*********,,,,,,,                      
                        ,,,,,,,*******,,,,,,,,        *,,,,,,,,,,****         ,,,,,,,,,*************,,,,,,.                     
                       .,,,,,,**********,,,,,,,,,     .*,,,,,,,,,,,,,,,**    ,,,,,,,****************,,,,,,,                     
                       ,,,,,,,************,,,,,,,,,    .*,,,,,,,,,,,,,,,,,**,,,,,,,*****************,,,,,,,                     
                       ,,,,,,,***************,,,,,,,,.**.,*,,,,,,,,,,,,,,,,,,,,,,,******************,,,,,,,                     
                       ,,,,,,,********,,,******,,,***,,,,,,,,,,,,,,,,,,,,,,,,,,,,*******************,,,,,,,                     
                       ,,,,,,,*********,,,,,,,,**,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,***,,,,***********,,,,,,,.                     
                       ,,,,,,,*********,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,***********,,,,,,,                      
                       ,,,,,,,,**********,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,***********,,,,,,,,                      
                       .,,,,,,,********,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,***********,,,,,,,,                       
                        ,,,,,,,,*****,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,*******,,,,,,,,,                        
                         ,,,,,,,,,**,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,****,,,,,,,,,                         
                          ,,,,,,,,,,,&&&&&&&&&&&&&&&&&&*,,,,,,,,,,,,,,&&&&&&&&&&&,,,,,,*,,,,,,,,,,,,,,                          
                           ,,,,,,,,,,,,,,,,,,&&&&&&&&&&*,,,,,,,,,,,,,,%&&&&&&&&&&,,,,,,,,,,,,,,,,,,,                            
                            ,,,,,,,,,,,,,,,,/&&&&&&&&&&,,,,,,,,,,,,,,,,&&&&&&&&&&,,,,,,,,,,,,,,,,,,,,,,,,.                      
                   .,,,,,,...,,,,,,,,,,,,,,,,&&&&&&&&&&,,,,,,,,,,,,,,,,&&&&&&&&&&,,,,,,,,,,,,,,,,,,,,,,,,                       
                     ,,,,,,,,,,,,,,,,,,,,,,,,%&&&&&&&&,,,,,,,,,,,,,,,,,,&&&&&&&&*,,,,,,,,,,,,,,,,,,,,,,                         
                      .,,,,,,,,,,,,,,,,,,,,,,,,#&&&&%,,,,,,,,,,,,,,,,,,,,,%&&%,,,,,,,,,,,,,,,,,,,,,,,                           
                         ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,//////,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                              
                           .  ..,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.,,,,,,,,..  ...,                             
                          ,,,,,,,,,,,,..  .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.....,,,,,,,,,,,,.                            
                         ,,,,,,, ...,,,,,,,,,,,,,,,,,&*,,,,&&%,(&&#*%&&,,,,,,,,,,,,,,,,,,... .,,,,,,,,.  .                      
                        .,,,. .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,, .,,,,,,                        
                         .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,   ..,,..                               
                                         .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,.                                           
                                        ,,,******,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,**                                               
                                          ,,,,,,,,,**************************,,,,,.                                             
                                              ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                            
                                               .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                           
                                             ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                          
                                           .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                         
                                                  ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                        
                                                 ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                        
                                                ,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                       
                                               .,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,                                      

*/