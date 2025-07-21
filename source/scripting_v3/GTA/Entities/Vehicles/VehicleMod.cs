//
// Copyright (C) 2015 crosire & kagikn & contributors
// License: https://github.com/scripthookvdotnet/scripthookvdotnet#license
//

using GTA.Native;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GTA
{
    public sealed class VehicleMod
    {
        #region Fields
        static readonly ReadOnlyDictionary<int, Tuple<string, string>> _hornNames = new(
            new Dictionary<int, Tuple<string, string>>
            {
                {-1,  new Tuple<string, string>("CMOD_HRN_0", "Stock Horn")},
                {0,  new Tuple<string, string>("CMOD_HRN_TRK","Truck Horn")},
                {1,  new Tuple<string, string>("CMOD_HRN_COP", "Cop Horn")},
                {2,  new Tuple<string, string>("CMOD_HRN_CLO", "Clown Horn")},
                {3,  new Tuple<string, string>("CMOD_HRN_MUS1", "Musical Horn 1")},
                {4,  new Tuple<string, string>("CMOD_HRN_MUS2", "Musical Horn 2")},
                {5,  new Tuple<string, string>("CMOD_HRN_MUS3", "Musical Horn 3")},
                {6,  new Tuple<string, string>("CMOD_HRN_MUS4", "Musical Horn 4")},
                {7,  new Tuple<string, string>("CMOD_HRN_MUS5", "Musical Horn 5")},
                {8,  new Tuple<string, string>("CMOD_HRN_SAD", "Sad Trombone")},
                {9,  new Tuple<string, string>("HORN_CLAS1", "Classical Horn 1")},
                {10,  new Tuple<string, string>("HORN_CLAS2", "Classical Horn 2")},
                {11,  new Tuple<string, string>("HORN_CLAS3", "Classical Horn 3")},
                {12,  new Tuple<string, string>("HORN_CLAS4", "Classical Horn 4")},
                {13,  new Tuple<string, string>("HORN_CLAS5", "Classical Horn 5")},
                {14,  new Tuple<string, string>("HORN_CLAS6", "Classical Horn 6")},
                {15,  new Tuple<string, string>("HORN_CLAS7", "Classical Horn 7")},
                {16,  new Tuple<string, string>("HORN_CNOTE_C0", "Scale Do")},
                {17,  new Tuple<string, string>("HORN_CNOTE_D0", "Scale Re")},
                {18,  new Tuple<string, string>("HORN_CNOTE_E0", "Scale Mi")},
                {19,  new Tuple<string, string>("HORN_CNOTE_F0", "Scale Fa")},
                {20,  new Tuple<string, string>("HORN_CNOTE_G0", "Scale Sol")},
                {21,  new Tuple<string, string>("HORN_CNOTE_A0", "Scale La")},
                {22,  new Tuple<string, string>("HORN_CNOTE_B0", "Scale Ti")},
                {23,  new Tuple<string, string>("HORN_CNOTE_C1", "Scale Do (High)")},
                {24,  new Tuple<string, string>("HORN_HIPS1", "Jazz Horn 1")},
                {25,  new Tuple<string, string>("HORN_HIPS2", "Jazz Horn 2")},
                {26,  new Tuple<string, string>("HORN_HIPS3", "Jazz Horn 3")},
                {27,  new Tuple<string, string>("HORN_HIPS4", "Jazz Horn Loop")},
                {28,  new Tuple<string, string>("HORN_INDI_1", "Star Spangled Banner 1")},
                {29,  new Tuple<string, string>("HORN_INDI_2", "Star Spangled Banner 2")},
                {30,  new Tuple<string, string>("HORN_INDI_3", "Star Spangled Banner 3")},
                {31,  new Tuple<string, string>("HORN_INDI_4", "Star Spangled Banner 4")},
                {32,  new Tuple<string, string>("HORN_LUXE2", "Classical Horn Loop 1")},
                {33,  new Tuple<string, string>("HORN_LUXE1", "Classical Horn 8")},
                {34,  new Tuple<string, string>("HORN_LUXE3", "Classical Horn Loop 2")},
                {35,  new Tuple<string, string>("HORN_LUXE2", "Classical Horn Loop 1")},
                {36,  new Tuple<string, string>("HORN_LUXE1", "Classical Horn 8")},
                {37,  new Tuple<string, string>("HORN_LUXE3", "Classical Horn Loop 2")},
                {38,  new Tuple<string, string>("HORN_HWEEN1", "Halloween Loop 1")},
                {39,  new Tuple<string, string>("HORN_HWEEN1", "Halloween Loop 1")},
                {40,  new Tuple<string, string>("HORN_HWEEN2", "Halloween Loop 2")},
                {41,  new Tuple<string, string>("HORN_HWEEN2", "Halloween Loop 2")},
                {42,  new Tuple<string, string>("HORN_LOWRDER1", "San Andreas Loop")},
                {43,  new Tuple<string, string>("HORN_LOWRDER1", "San Andreas Loop")},
                {44,  new Tuple<string, string>("HORN_LOWRDER2", "Liberty City Loop")},
                {45,  new Tuple<string, string>("HORN_LOWRDER2", "Liberty City Loop")},
                {46,  new Tuple<string, string>("HORN_XM15_1", "Festive Loop 1")},
                {47,  new Tuple<string, string>("HORN_XM15_2", "Festive Loop 2")},
                {48,  new Tuple<string, string>("HORN_XM15_3", "Festive Loop 3")}
            });
        #endregion

        internal VehicleMod(Vehicle owner, VehicleModType modType)
        {
            Vehicle = owner;
            Type = modType;
        }

        public Vehicle Vehicle
        {
            get;
        }

        public VehicleModType Type
        {
            get;
        }

        public int Count => Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, Vehicle.Handle, (int)Type);

        public int Index
        {
            get => Function.Call<int>(Hash.GET_VEHICLE_MOD, Vehicle.Handle, (int)Type);
            set => Function.Call(Hash.SET_VEHICLE_MOD, Vehicle.Handle, (int)Type, value, Variation);
        }

        public void Remove()
        {
            Function.Call(Hash.REMOVE_VEHICLE_MOD, Vehicle.Handle, (int)Type);
        }

        public bool Variation
        {
            get => Function.Call<bool>(Hash.GET_VEHICLE_MOD_VARIATION, Vehicle.Handle, (int)Type);
            set => Function.Call(Hash.SET_VEHICLE_MOD, Vehicle.Handle, (int)Type, Index, value);
        }

        public string LocalizedName
        {
            get
            {
                int index = Index;
                int count = Count;
                // This still needs a little more work, but it is better than what it used to be
                if (count == 0)
                {
                    return string.Empty;
                }

                if (index < -1 || index >= count)
                {
                    return string.Empty;
                }

                if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
                {
                    Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                    Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
                }
                string cur;
                if (Type == VehicleModType.Horns)
                {
                    if (!_hornNames.ContainsKey(index))
                    {
                        return string.Empty;
                    }

                    if (string.IsNullOrEmpty(Game.GetLocalizedString(_hornNames[index].Item1)))
                    {
                        return _hornNames[index].Item2;
                    }

                    return Game.GetLocalizedString(_hornNames[index].Item1);
                }
                if (Type == VehicleModType.FrontWheel || Type == VehicleModType.RearWheel)
                {
                    if (index == -1)
                    {
                        if (!Vehicle.Model.IsBike && Vehicle.Model.IsBicycle)
                        {
                            return Game.GetLocalizedString("CMOD_WHE_0");
                        }
                        else
                        {
                            return Game.GetLocalizedString("CMOD_WHE_B_0");
                        }
                    }
                    if (index >= count / 2)
                    {
                        return Game.GetLocalizedString("CHROME") + " " +
                               Game.GetLocalizedString(Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, Vehicle.Handle, (int)Type, index));
                    }
                    else
                    {
                        return Game.GetLocalizedString(Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, Vehicle.Handle, (int)Type, index));
                    }
                }

                switch (Type)
                {
                    case VehicleModType.Armor:
                        return Game.GetLocalizedString("CMOD_ARM_" + (index + 1).ToString());
                    case VehicleModType.Brakes:
                        return Game.GetLocalizedString("CMOD_BRA_" + (index + 1).ToString());
                    case VehicleModType.Engine:
                        if (index == -1)
                        {
                            //Engine doesn't list anything in LSC for no parts, but there is a setting with no part. so just use armours none
                            return Game.GetLocalizedString("CMOD_ARM_0");
                        }
                        return Game.GetLocalizedString("CMOD_ENG_" + (index + 2).ToString());
                    case VehicleModType.Suspension:
                        return Game.GetLocalizedString("CMOD_SUS_" + (index + 1).ToString());
                    case VehicleModType.Transmission:
                        return Game.GetLocalizedString("CMOD_GBX_" + (index + 1).ToString());
                }
                if (index > -1)
                {
                    cur = Function.Call<string>(Hash.GET_MOD_TEXT_LABEL, Vehicle.Handle, (int)Type, index);
                    if (!string.IsNullOrEmpty(Game.GetLocalizedString(cur)))
                    {
                        cur = Game.GetLocalizedString(cur);
                        if (cur == "" || cur == "NULL")
                        {
                            return LocalizedTypeName + " " + (index + 1).ToString();
                        }
                        return cur;
                    }
                    return LocalizedTypeName + " " + (index + 1).ToString();
                }
                else
                {
                    switch (Type)
                    {
                        case VehicleModType.AirFilter:
                            if (Vehicle.Model == VehicleHash.Tornado)
                            {
                            }
                            break;
                        case VehicleModType.Struts:
                            switch ((VehicleHash)Vehicle.Model)
                            {
                                case VehicleHash.Banshee:
                                case VehicleHash.Banshee2:
                                case VehicleHash.SultanRS:
                                    return Game.GetLocalizedString("CMOD_COL5_41");
                            }
                            break;

                    }
                    return Game.GetLocalizedString("CMOD_DEF_0");
                }
            }
        }

        public string LocalizedTypeName
        {
            get
            {
                if (!Function.Call<bool>(Hash.HAS_THIS_ADDITIONAL_TEXT_LOADED, "mod_mnu", 10))
                {
                    Function.Call(Hash.CLEAR_ADDITIONAL_TEXT, 10, true);
                    Function.Call(Hash.REQUEST_ADDITIONAL_TEXT, "mod_mnu", 10);
                }
                string cur = string.Empty;
                switch (Type)
                {
                    case VehicleModType.Armor:
                        cur = Game.GetLocalizedString("CMOD_MOD_ARM");
                        break;
                    case VehicleModType.Brakes:
                        cur = Game.GetLocalizedString("CMOD_MOD_BRA");
                        break;
                    case VehicleModType.Engine:
                        cur = Game.GetLocalizedString("CMOD_MOD_ENG");
                        break;
                    case VehicleModType.Suspension:
                        cur = Game.GetLocalizedString("CMOD_MOD_SUS");
                        break;
                    case VehicleModType.Transmission:
                        cur = Game.GetLocalizedString("CMOD_MOD_TRN");
                        break;
                    case VehicleModType.Horns:
                        cur = Game.GetLocalizedString("CMOD_MOD_HRN");
                        break;
                    case VehicleModType.FrontWheel:
                        if (!Vehicle.Model.IsBike && Vehicle.Model.IsBicycle)
                        {
                            cur = Game.GetLocalizedString("CMOD_MOD_WHEM");
                            if (cur == string.Empty)
                            {
                                return "Wheels";
                            }
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMOD_WHE0_0");
                        }

                        break;
                    case VehicleModType.RearWheel:
                        cur = Game.GetLocalizedString("CMOD_WHE0_1");
                        break;

                    //Benny's
                    case VehicleModType.PlateHolder:
                        cur = Game.GetLocalizedString("CMM_MOD_S0");
                        break;
                    case VehicleModType.VanityPlates:
                        cur = Game.GetLocalizedString("CMM_MOD_S1");
                        break;
                    case VehicleModType.TrimDesign:
                        if (Vehicle.Model == VehicleHash.SultanRS)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S2b");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S2");
                        }
                        break;
                    case VehicleModType.Ornaments:
                        cur = Game.GetLocalizedString("CMM_MOD_S3");
                        break;
                    case VehicleModType.Dashboard:
                        cur = Game.GetLocalizedString("CMM_MOD_S4");
                        break;
                    case VehicleModType.DialDesign:
                        cur = Game.GetLocalizedString("CMM_MOD_S5");
                        break;
                    case VehicleModType.DoorSpeakers:
                        cur = Game.GetLocalizedString("CMM_MOD_S6");
                        break;
                    case VehicleModType.Seats:
                        cur = Game.GetLocalizedString("CMM_MOD_S7");
                        break;
                    case VehicleModType.SteeringWheels:
                        cur = Game.GetLocalizedString("CMM_MOD_S8");
                        break;
                    case VehicleModType.ColumnShifterLevers:
                        cur = Game.GetLocalizedString("CMM_MOD_S9");
                        break;
                    case VehicleModType.Plaques:
                        cur = Game.GetLocalizedString("CMM_MOD_S10");
                        break;
                    case VehicleModType.Speakers:
                        cur = Game.GetLocalizedString("CMM_MOD_S11");
                        break;
                    case VehicleModType.Trunk:
                        cur = Game.GetLocalizedString("CMM_MOD_S12");
                        break;
                    case VehicleModType.Hydraulics:
                        cur = Game.GetLocalizedString("CMM_MOD_S13");
                        break;
                    case VehicleModType.EngineBlock:
                        cur = Game.GetLocalizedString("CMM_MOD_S14");
                        break;
                    case VehicleModType.AirFilter:
                        if (Vehicle.Model == VehicleHash.SultanRS)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S15b");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S15");
                        }
                        break;
                    case VehicleModType.Struts:
                        if (Vehicle.Model == VehicleHash.SultanRS || Vehicle.Model == VehicleHash.Banshee2)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S16b");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S16");
                        }

                        break;
                    case VehicleModType.ArchCover:
                        if (Vehicle.Model == VehicleHash.SultanRS)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S17b");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S17");
                        }
                        break;
                    case VehicleModType.Aerials:
                        if (Vehicle.Model == VehicleHash.SultanRS)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S18b");
                        }
                        else if (Vehicle.Model == VehicleHash.BType3)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S18c");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S18");
                        }
                        break;
                    case VehicleModType.Trim:
                        if (Vehicle.Model == VehicleHash.SultanRS)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S19b");
                        }
                        else if (Vehicle.Model == VehicleHash.BType3)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S19c");
                        }
                        else if (Vehicle.Model == VehicleHash.Virgo2)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S19d");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S19");
                        }
                        break;
                    case VehicleModType.Tank:
                        if (Vehicle.Model == VehicleHash.SlamVan3)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S27");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S20");
                        }
                        break;

                    case VehicleModType.Windows:
                        if (Vehicle.Model == VehicleHash.BType3)
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S21b");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S21");
                        }
                        break;
                    case (VehicleModType)47:
                        if (Vehicle.Model == VehicleHash.SlamVan3)
                        {
                            cur = Game.GetLocalizedString("SLVAN3_RDOOR");
                        }
                        else
                        {
                            cur = Game.GetLocalizedString("CMM_MOD_S22");
                        }
                        break;
                    case VehicleModType.Livery:
                        cur = Game.GetLocalizedString("CMM_MOD_S23");
                        break;

                    default:
                        cur = Function.Call<string>(Hash.GET_MOD_SLOT_NAME, Vehicle.Handle, (int)Type);
                        if (!string.IsNullOrEmpty(Game.GetLocalizedString(cur)))
                        {
                            cur = Game.GetLocalizedString(cur);
                        }
                        break;
                }
                if (cur == string.Empty)
                {
                    cur = Type.ToString();  //would only happen if the text isn't loaded
                }
                return cur;

            }
        }
    }
}
