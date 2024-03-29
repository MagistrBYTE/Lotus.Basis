using System;
using System.IO;
using System.Reflection;

namespace Lotus.Core.Serialization
{
    /** \addtogroup CoreSerialization
	*@{*/
    /// <summary>
    /// Статический класс реализующий сериализацию примитивных данных в бинарный поток.
    /// </summary>
    public static class XSerializatorBinaryPrimitive
    {
        /// <summary>
        /// Запись объекта примитивного типа в бинарный поток.
        /// </summary>
        /// <param name="writer">Средство записи данных в формат XML.</param>
        /// <param name="type">Тип объекта.</param>
        /// <param name="instance">Экземпляр объекта.</param>
        public static void WriteToBinary(BinaryWriter writer, Type type, object instance)
        {
            switch (type.Name)
            {
                //
                //
                //
                case nameof(Boolean):
                    {
                        writer.Write((bool)instance);
                    }
                    break;
                case nameof(Byte):
                    {
                        writer.Write((byte)instance);
                    }
                    break;
                case nameof(Char):
                    {
                        writer.Write((char)instance);
                    }
                    break;
                case nameof(Int16):
                    {
                        writer.Write((short)instance);
                    }
                    break;
                case nameof(UInt16):
                    {
                        writer.Write((ushort)instance);
                    }
                    break;
                case nameof(Int32):
                    {
                        writer.Write((int)instance);
                    }
                    break;
                case nameof(UInt32):
                    {
                        writer.Write((uint)instance);
                    }
                    break;
                case nameof(Int64):
                    {
                        writer.Write((long)instance);
                    }
                    break;
                case nameof(UInt64):
                    {
                        writer.Write((long)(ulong)instance);
                    }
                    break;
                case nameof(Single):
                    {
                        writer.Write((float)instance);
                    }
                    break;
                case nameof(Double):
                    {
                        writer.Write((double)instance);
                    }
                    break;
                case nameof(Decimal):
                    {
                        writer.Write((decimal)instance);
                    }
                    break;
                case nameof(String):
                    {
                        writer.Write((string)instance);
                    }
                    break;
                case nameof(DateTime):
                    {
                        writer.Write((DateTime)instance);
                    }
                    break;
                case nameof(TimeSpan):
                    {
                        writer.Write(((TimeSpan)instance).Ticks);
                    }
                    break;
                case nameof(Version):
                    {
                        writer.Write(((Version)instance).ToString());
                    }
                    break;
                case nameof(Uri):
                    {
                        writer.Write(((Uri)instance).ToString());
                    }
                    break;

                case nameof(TColor):
                    {
                        var color = (TColor)instance;
                        writer.Write(color.ToRGBA());
                    }
                    break;
                case nameof(CVariant):
                    {
                        var variant = (CVariant)instance;
                        writer.Write(variant.SerializeToString());
                    }
                    break;
                //
                // UnityEngine
                //
#if UNITY_2017_1_OR_NEWER
				case nameof(UnityEngine.Vector2):
					{
						var vector = (UnityEngine.Vector2)instance;
						writer.Write(vector.x);
						writer.Write(vector.y);
					}
					break;
				case nameof(UnityEngine.Vector3):
					{
						var vector = (UnityEngine.Vector3)instance;
						writer.Write(vector.x);
						writer.Write(vector.y);
						writer.Write(vector.z);
					}
					break;
				case nameof(UnityEngine.Vector4):
					{
						var vector = (UnityEngine.Vector4)instance;
						writer.Write(vector.x);
						writer.Write(vector.y);
						writer.Write(vector.z);
						writer.Write(vector.w);
					}
					break;
				case nameof(UnityEngine.Vector2Int):
					{
						var vector = (UnityEngine.Vector2Int)instance;
						writer.Write(vector.x);
						writer.Write(vector.y);
					}
					break;
				case nameof(UnityEngine.Vector3Int):
					{
						var vector = (UnityEngine.Vector3Int)instance;
						writer.Write(vector.x);
						writer.Write(vector.y);
						writer.Write(vector.z);
					}
					break;
				case nameof(UnityEngine.Quaternion):
					{
						var quaternion = (UnityEngine.Quaternion)instance;
						writer.Write(quaternion.x);
						writer.Write(quaternion.y);
						writer.Write(quaternion.z);
						writer.Write(quaternion.w);
					}
					break;
				case nameof(UnityEngine.Color):
					{
						var color = (UnityEngine.Color)instance;
						writer.Write(color.ToRGBA());
					}
					break;
				case nameof(UnityEngine.Color32):
					{
						var color = (UnityEngine.Color32)instance;
						writer.Write(color.ToRGBA());
					}
					break;
				case nameof(UnityEngine.Rect):
					{
						var rect = (UnityEngine.Rect)instance;
						writer.Write(rect.x);
						writer.Write(rect.y);
						writer.Write(rect.width);
						writer.Write(rect.height);
					}
					break;
				case nameof(UnityEngine.RectInt):
					{
						var rect = (UnityEngine.RectInt)instance;
						writer.Write(rect.x);
						writer.Write(rect.y);
						writer.Write(rect.width);
						writer.Write(rect.height);
					}
					break;
				case nameof(UnityEngine.Bounds):
					{
						var bounds = (UnityEngine.Bounds)instance;
						writer.Write(bounds.min.x);
						writer.Write(bounds.min.y);
						writer.Write(bounds.min.z);
						writer.Write(bounds.max.x);
						writer.Write(bounds.max.y);
						writer.Write(bounds.max.z);
					}
					break;
				case nameof(UnityEngine.BoundsInt):
					{
						var bounds = (UnityEngine.BoundsInt)instance;
						writer.Write(bounds.min.x);
						writer.Write(bounds.min.y);
						writer.Write(bounds.min.z);
						writer.Write(bounds.max.x);
						writer.Write(bounds.max.y);
						writer.Write(bounds.max.z);
					}
					break;
#endif
                default:
                    {
                        // Проверка на перечисление
                        if (type.IsEnum)
                        {
                            writer.Write((int)instance);
                            break;
                        }

                        // Проверка на примитивный тип
                        if (type.HasAttribute<LotusSerializeAsPrimitiveAttribute>())
                        {
                            var method_info = type.GetMethod(
                                LotusSerializeAsPrimitiveAttribute.SERIALIZE_TO_STRING,
                                BindingFlags.Public | BindingFlags.Instance);
                            if (method_info != null)
                            {
                                var data = method_info.Invoke(instance, null)?.ToString();
                                writer.Write(data!);
                            }
                        }
                    }
                    break;
            }
        }
    }
    /**@}*/
}