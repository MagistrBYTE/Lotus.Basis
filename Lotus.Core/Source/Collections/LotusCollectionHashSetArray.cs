//=====================================================================================================================
// Проект: Модуль базового ядра
// Раздел: Подсистема коллекций
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusCollectionHashSetArray.cs
*		HashSetArray на основе массива.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace Lotus
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreCollections
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// HashSetArray на основе массива
		/// </summary>
		/// <typeparam name="TItem">Тип элемента</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class HashSetArray<TItem> : ICollection<TItem>, ISet<TItem>, IReadOnlyCollection<TItem>
		{
			#region ======================================= ВНУТРЕННИЕ ТИПЫ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Тип реализующий перечислителя по списку
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public struct HashSetArrayEnumerator : IEnumerator<TItem>, IEnumerator
			{
				#region ======================================= ДАННЫЕ ================================================
				private HashSetArray<TItem> mSet;
				private Int32 mIndex;
				private Int32 mVersion;
				private TItem mCurrent;
				#endregion

				#region ======================================= СВОЙСТВА ==============================================
				/// <summary>
				/// Текущий элемент
				/// </summary>
				public TItem Current
				{
					get
					{
						return mCurrent;
					}
				}

				/// <summary>
				/// Текущий элемент
				/// </summary>
				Object IEnumerator.Current
				{
					get
					{
						return Current;
					}
				}
				#endregion

				#region ======================================= КОНСТРУКТОРЫ ==========================================
				//-----------------------------------------------------------------------------------------------------
				/// <summary>
				/// Конструктор инициализирует данные перечислителя указанным списком
				/// </summary>
				/// <param name="set">Список</param>
				//-----------------------------------------------------------------------------------------------------
				internal HashSetArrayEnumerator(HashSetArray<TItem> set)
				{
					this.mSet = set;
					mIndex = 0;
					mVersion = set.mVersion;
					mCurrent = default(TItem);
				}
				#endregion

				#region ======================================= ОБЩИЕ МЕТОДЫ ==========================================
				//-----------------------------------------------------------------------------------------------------
				/// <summary>
				/// Освобождение управляемых ресурсов
				/// </summary>
				//-----------------------------------------------------------------------------------------------------
				public void Dispose()
				{
				}

				//-----------------------------------------------------------------------------------------------------
				/// <summary>
				/// Переход к следующему элементу списка
				/// </summary>
				/// <returns>Возможность перехода к следующему элементу списка</returns>
				//-----------------------------------------------------------------------------------------------------
				public Boolean MoveNext()
				{
					if (mVersion != mSet.mVersion)
					{
					}

					while (mIndex < mSet.mLastIndex)
					{
						if (mSet.mSlots[mIndex].hashCode >= 0)
						{
							mCurrent = mSet.mSlots[mIndex].value;
							mIndex++;
							return true;
						}
						mIndex++;
					}
					mIndex = mSet.mLastIndex + 1;
					mCurrent = default(TItem);
					return false;
				}

				//-----------------------------------------------------------------------------------------------------
				/// <summary>
				/// Перестановка позиции на первый элемент списка
				/// </summary>
				//-----------------------------------------------------------------------------------------------------
				void IEnumerator.Reset()
				{
					if (mVersion != mSet.mVersion)
					{
					}

					mIndex = 0;
					mCurrent = default(TItem);
				}
				#endregion
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный тип
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected internal struct Slot
			{
				internal Int32 hashCode;	  // Lower 31 bits of hash code, -1 if unused
				internal Int32 next;		  // Index of next entry, -1 if last
				internal TItem value;
			}
			#endregion

			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			// store lower 31 bits of hash code
			private const Int32 Lower31BitMask = 0x7FFFFFFF;
			
			// cutoff poInt32, above which we won't do stackallocs. This corresponds to 100 integers.
			private const Int32 StackAllocThreshold = 100;
			
			// when constructing a hashset from an existing collection, it may contain duplicates, 
			// so this is used as the max acceptable excess ratio of capacity to count. Note that
			// this is only used on the ctor and not to automatically shrink if the hashset has, e.g,
			// a lot of adds followed by removes. Users must explicitly shrink by calling TrimExcess.
			// This is set to 3 because capacity is acceptable as 2x rounded up to nearest prime.
			private const Int32 ShrinkThreshold = 3;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal Int32[] mBuckets;
			protected internal Slot[] mSlots;
			protected internal Int32 mCount;
			protected internal Int32 mLastIndex;
			protected internal Int32 mFreeList;
			protected internal IEqualityComparer<TItem> mComparer;
			protected internal Int32 mVersion;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Number of elements in this hashset
			/// </summary>
			public Int32 Count
			{
				get { return mCount; }
			}

			/// <summary>
			/// Whether this is readonly
			/// </summary>
			Boolean ICollection<TItem>.IsReadOnly
			{
				get { return false; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray()
				: this(EqualityComparer<TItem>.Default) 
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="capacity">Начальная максимальная емкость списка</param>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray(Int32 capacity)
				: this(capacity, EqualityComparer<TItem>.Default) 
			{ 
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="comparer">Компаратор</param>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray(IEqualityComparer<TItem> comparer)
			{
				if (comparer == null)
				{
					comparer = EqualityComparer<TItem>.Default;
				}

				this.mComparer = comparer;
				mLastIndex = 0;
				mCount = 0;
				mFreeList = -1;
				mVersion = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="collection">Список элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray(IEnumerable<TItem> collection)
				: this(collection, EqualityComparer<TItem>.Default) 
			{ 
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="collection">Список элементов</param>
			/// <param name="comparer">Компаратор</param>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray(IEnumerable<TItem> collection, IEqualityComparer<TItem> comparer)
				: this(comparer)
			{
				var otherAsHashSet = collection as HashSetArray<TItem>;
				if (otherAsHashSet != null && AreEqualityComparersEqual(this, otherAsHashSet))
				{
					CopyFrom(otherAsHashSet);
				}
				else
				{
					// to avoid excess resizes, first set size based on collection's count. Collection
					// may contain duplicates, so call TrimExcess if resulting hashset is larger than
					// threshold
					ICollection<TItem> coll = collection as ICollection<TItem>;
					Int32 suggestedCapacity = coll == null ? 0 : coll.Count;
					Initialize(suggestedCapacity);

					this.UnionWith(collection);

					if (mCount > 0 && mSlots.Length / mCount > ShrinkThreshold)
					{
						TrimExcess();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="capacity">Начальная максимальная емкость списка</param>
			/// <param name="comparer">Компаратор</param>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArray(Int32 capacity, IEqualityComparer<TItem> comparer)
				: this(comparer)
			{
				if (capacity > 0)
				{
					Initialize(capacity);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ICollection ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Add item to this hashset. This is the explicit implementation of the ICollection
			/// interface. The other Add method returns bool indicating whether item was added.
			/// </summary>
			/// <param name="item">item to add</param>
			//---------------------------------------------------------------------------------------------------------
			void ICollection<TItem>.Add(TItem item)
			{
				AddIfNotPresent(item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Remove all items from this set. This clears the elements but not the underlying 
			/// buckets and slots array. Follow this call by TrimExcess to release these.
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				if (mLastIndex > 0)
				{
					// clear the elements so that the gc can reclaim the references.
					// clear only up to mLastIndex for mSlots 
					Array.Clear(mSlots, 0, mLastIndex);
					Array.Clear(mBuckets, 0, mBuckets.Length);
					mLastIndex = 0;
					mCount = 0;
					mFreeList = -1;
				}
				mVersion++;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this hashset contains the item
			/// </summary>
			/// <param name="item">item to check for containment</param>
			/// <returns>true if item contained; false if not</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Contains(TItem item)
			{
				if (mBuckets != null)
				{
					Int32 hashCode = InternalGetHashCode(item);
					// see note at "HashSetArray" level describing why "- 1" appears in for loop
					for (Int32 i = mBuckets[hashCode % mBuckets.Length] - 1; i >= 0; i = mSlots[i].next)
					{
						if (mSlots[i].hashCode == hashCode && mComparer.Equals(mSlots[i].value, item))
						{
							return true;
						}
					}
				}
				// either mBuckets is null or wasn't found
				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Copy items in this hashset to array, starting at arrayIndex
			/// </summary>
			/// <param name="array">array to add items to</param>
			/// <param name="arrayIndex">index to start at</param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyTo(TItem[] array, Int32 arrayIndex)
			{
				CopyTo(array, arrayIndex, mCount);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Remove item from this hashset
			/// </summary>
			/// <param name="item">item to remove</param>
			/// <returns>true if removed; false if not (i.e. if the item wasn't in the HashSetArray)</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Remove(TItem item)
			{
				if (mBuckets != null)
				{
					Int32 hashCode = InternalGetHashCode(item);
					Int32 bucket = hashCode % mBuckets.Length;
					Int32 last = -1;
					for (Int32 i = mBuckets[bucket] - 1; i >= 0; last = i, i = mSlots[i].next)
					{
						if (mSlots[i].hashCode == hashCode && mComparer.Equals(mSlots[i].value, item))
						{
							if (last < 0)
							{
								// first iteration; update buckets
								mBuckets[bucket] = mSlots[i].next + 1;
							}
							else
							{
								// subsequent iterations; update 'next' pointers
								mSlots[last].next = mSlots[i].next;
							}
							mSlots[i].hashCode = -1;
							mSlots[i].value = default(TItem);
							mSlots[i].next = mFreeList;

							mCount--;
							mVersion++;
							if (mCount == 0)
							{
								mLastIndex = 0;
								mFreeList = -1;
							}
							else
							{
								mFreeList = i;
							}
							return true;
						}
					}
				}
				// either mBuckets is null or wasn't found
				return false;
			}
			#endregion

			#region ======================================= МЕТОДЫ IEnumerable ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			public HashSetArrayEnumerator GetEnumerator()
			{
				return new HashSetArrayEnumerator(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
			{
				return new HashSetArrayEnumerator(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			IEnumerator IEnumerable.GetEnumerator()
			{
				return new HashSetArrayEnumerator(this);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Add item to this HashSetArray. Returns bool indicating whether item was added (won't be 
			/// added if already present)
			/// </summary>
			/// <param name="item"></param>
			/// <returns>true if added, false if already present</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Add(TItem item)
			{
				return AddIfNotPresent(in item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Add item to this HashSetArray. Returns bool indicating whether item was added (won't be 
			/// added if already present)
			/// </summary>
			/// <param name="item"></param>
			/// <returns>true if added, false if already present</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Add(in TItem item)
			{
				return AddIfNotPresent(in item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Searches the set for a given value and returns the equal value it finds, if any.
			/// </summary>
			/// <param name="equalValue">The value to search for.</param>
			/// <param name="actualValue">The value from the set that the search found, or the default value of <typeparamref name="TItem"/> when the search yielded no match.</param>
			/// <returns>A value indicating whether the search was successful.</returns>
			/// <remarks>
			/// This can be useful when you want to reuse a previously stored reference instead of 
			/// a newly constructed one (so that more sharing of references can occur) or to look up
			/// a value that has more complete data than the value you currently have, although their
			/// comparer functions indicate they are equal.
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public Boolean TryGetValue(in TItem equalValue, out TItem actualValue)
			{
				if (mBuckets != null)
				{
					Int32 i = InternalIndexOf(equalValue);
					if (i >= 0)
					{
						actualValue = mSlots[i].value;
						return true;
					}
				}
				actualValue = default(TItem);
				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Take the union of this HashSetArray with other. Modifies this set.
			/// 
			/// Implementation note: GetSuggestedCapacity (to increase capacity in advance avoiding 
			/// multiple resizes ended up not being useful in practice; quickly gets to the 
			/// poInt32 where it's a wasteful check.
			/// </summary>
			/// <param name="other">enumerable with items to add</param>
			//---------------------------------------------------------------------------------------------------------
			public void UnionWith(IEnumerable<TItem> other)
			{
				foreach (TItem item in other)
				{
					AddIfNotPresent(item);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Takes the intersection of this set with other. Modifies this set.
			/// 
			/// Implementation Notes: 
			/// We get better perf if other is a hashset using same equality comparer, because we 
			/// get constant contains check in other. Resulting cost is O(n1) to iterate over this.
			/// 
			/// If we can't go above route, iterate over the other and mark intersection by checking
			/// contains in this. Then loop over and delete any unmarked elements. Total cost is n2+n1. 
			/// 
			/// Attempts to return early based on counts alone, using the property that the 
			/// intersection of anything with the empty set is the empty set.
			/// </summary>
			/// <param name="other">enumerable with items to add </param>
			//---------------------------------------------------------------------------------------------------------
			public void IntersectWith(IEnumerable<TItem> other)
			{
				// intersection of anything with empty set is empty set, so return if count is 0
				if (mCount == 0)
				{
					return;
				}

				// if other is empty, intersection is empty set; remove all elements and we're done
				// can only figure this out if implements ICollection<TItem>. (IEnumerable<TItem> has no count)
				ICollection<TItem> otherAsCollection = other as ICollection<TItem>;
				if (otherAsCollection != null)
				{
					if (otherAsCollection.Count == 0)
					{
						Clear();
						return;
					}

					HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
					// faster if other is a hashset using same equality comparer; so check 
					// that other is a hashset using the same equality comparer.
					if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
					{
						IntersectWithHashSetWithSameEC(otherAsSet);
						return;
					}
				}

				//IntersectWithEnumerable(other);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Remove items in other from this set. Modifies this set.
			/// </summary>
			/// <param name="other">enumerable with items to remove</param>
			//---------------------------------------------------------------------------------------------------------
			public void ExceptWith(IEnumerable<TItem> other)
			{
				// this is already the enpty set; return
				if (mCount == 0)
				{
					return;
				}

				// special case if other is this; a set minus itself is the empty set
				if (other == this)
				{
					Clear();
					return;
				}

				// remove every element in other from this
				foreach (TItem element in other)
				{
					Remove(element);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Takes symmetric difference (XOR) with other and this set. Modifies this set.
			/// </summary>
			/// <param name="other">enumerable with items to XOR</param>
			//---------------------------------------------------------------------------------------------------------
			public void SymmetricExceptWith(IEnumerable<TItem> other)
			{
				// if set is empty, then symmetric difference is other
				if (mCount == 0)
				{
					UnionWith(other);
					return;
				}

				// special case this; the symmetric difference of a set with itself is the empty set
				if (other == this)
				{
					Clear();
					return;
				}

				HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
				// If other is a HashSetArray, it has unique elements according to its equality comparer,
				// but if they're using different equality comparers, then assumption of uniqueness
				// will fail. So first check if other is a hashset using the same equality comparer;
				// symmetric except is a lot faster and avoids bit array allocations if we can assume
				// uniqueness
				if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
				{
					SymmetricExceptWithUniqueHashSet(otherAsSet);
				}
				else
				{
					//SymmetricExceptWithEnumerable(other);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this is a subset of other.
			/// 
			/// Implementation Notes:
			/// The following properties are used up-front to avoid element-wise checks:
			/// 1. If this is the empty set, then it's a subset of anything, including the empty set
			/// 2. If other has unique elements according to this equality comparer, and this has more
			/// elements than other, then it can't be a subset.
			/// 
			/// Furthermore, if other is a hashset using the same equality comparer, we can use a 
			/// faster element-wise check.
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if this is a subset of other; false if not</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsSubsetOf(IEnumerable<TItem> other)
			{
				// The empty set is a subset of any set
				if (mCount == 0)
				{
					return true;
				}

				HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
				// faster if other has unique elements according to this equality comparer; so check 
				// that other is a hashset using the same equality comparer.
				if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
				{
					// if this has more elements then it can't be a subset
					if (mCount > otherAsSet.Count)
					{
						return false;
					}

					// already checked that we're using same equality comparer. simply check that 
					// each element in this is contained in other.
					return IsSubsetOfHashSetWithSameEC(otherAsSet);
				}
				else
				{
					//ElementCount result = CheckUniqueAndUnfoundElements(other, false);
					//return (result.uniqueCount == mCount && result.unfoundCount >= 0);
					return false;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this is a proper subset of other (i.e. strictly contained in)
			/// 
			/// Implementation Notes:
			/// The following properties are used up-front to avoid element-wise checks:
			/// 1. If this is the empty set, then it's a proper subset of a set that contains at least
			/// one element, but it's not a proper subset of the empty set.
			/// 2. If other has unique elements according to this equality comparer, and this has >=
			/// the number of elements in other, then this can't be a proper subset.
			/// 
			/// Furthermore, if other is a hashset using the same equality comparer, we can use a 
			/// faster element-wise check.
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if this is a proper subset of other; false if not</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsProperSubsetOf(IEnumerable<TItem> other)
			{
				 ICollection<TItem> otherAsCollection = other as ICollection<TItem>;
				if (otherAsCollection != null)
				{
					// the empty set is a proper subset of anything but the empty set
					if (mCount == 0)
					{
						return otherAsCollection.Count > 0;
					}
					HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
					// faster if other is a hashset (and we're using same equality comparer)
					if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
					{
						if (mCount >= otherAsSet.Count)
						{
							return false;
						}
						// this has strictly less than number of items in other, so the following
						// check suffices for proper subset.
						return IsSubsetOfHashSetWithSameEC(otherAsSet);
					}
				}

				//ElementCount result = CheckUniqueAndUnfoundElements(other, false);
				//return (result.uniqueCount == mCount && result.unfoundCount > 0);
				return false;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this is a superset of other
			/// 
			/// Implementation Notes:
			/// The following properties are used up-front to avoid element-wise checks:
			/// 1. If other has no elements (it's the empty set), then this is a superset, even if this
			/// is also the empty set.
			/// 2. If other has unique elements according to this equality comparer, and this has less 
			/// than the number of elements in other, then this can't be a superset
			/// 
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if this is a superset of other; false if not</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsSupersetOf(IEnumerable<TItem> other)
			{
				 // try to fall out early based on counts
				ICollection<TItem> otherAsCollection = other as ICollection<TItem>;
				if (otherAsCollection != null)
				{
					// if other is the empty set then this is a superset
					if (otherAsCollection.Count == 0)
					{
						return true;
					}
					HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
					// try to compare based on counts alone if other is a hashset with
					// same equality comparer
					if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
					{
						if (otherAsSet.Count > mCount)
						{
							return false;
						}
					}
				}

				return ContainsAllElements(other);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this is a proper superset of other (i.e. other strictly contained in this)
			/// 
			/// Implementation Notes: 
			/// This is slightly more complicated than above because we have to keep track if there
			/// was at least one element not contained in other.
			/// 
			/// The following properties are used up-front to avoid element-wise checks:
			/// 1. If this is the empty set, then it can't be a proper superset of any set, even if 
			/// other is the empty set.
			/// 2. If other is an empty set and this contains at least 1 element, then this is a proper
			/// superset.
			/// 3. If other has unique elements according to this equality comparer, and other's count
			/// is greater than or equal to this count, then this can't be a proper superset
			/// 
			/// Furthermore, if other has unique elements according to this equality comparer, we can
			/// use a faster element-wise check.
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if this is a proper superset of other; false if not</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsProperSupersetOf(IEnumerable<TItem> other)
			{
				// the empty set isn't a proper superset of any set.
				if (mCount == 0)
				{
					return false;
				}

				ICollection<TItem> otherAsCollection = other as ICollection<TItem>;
				if (otherAsCollection != null)
				{
					// if other is the empty set then this is a superset
					if (otherAsCollection.Count == 0)
					{
						// note that this has at least one element, based on above check
						return true;
					}
					HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
					// faster if other is a hashset with the same equality comparer
					if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
					{
						if (otherAsSet.Count >= mCount)
						{
							return false;
						}
						// now perform element check
						return ContainsAllElements(otherAsSet);
					}
				}
				// couldn't fall out in the above cases; do it the long way
				//ElementCount result = CheckUniqueAndUnfoundElements(other, true);
				//return (result.uniqueCount < mCount && result.unfoundCount == 0);
				return false;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this set overlaps other (i.e. they share at least one item)
			/// </summary>
			/// <param name="other"></param>
			/// <returns>true if these have at least one common element; false if disjoInt32</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Overlaps(IEnumerable<TItem> other)
			{
				if (mCount == 0)
				{
					return false;
				}

				foreach (TItem element in other)
				{
					if (Contains(element))
					{
						return true;
					}
				}
				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this and other contain the same elements. This is set equality: 
			/// duplicates and order are ignored
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean SetEquals(IEnumerable<TItem> other)
			{
				HashSetArray<TItem> otherAsSet = other as HashSetArray<TItem>;
				// faster if other is a hashset and we're using same equality comparer
				if (otherAsSet != null && AreEqualityComparersEqual(this, otherAsSet))
				{
					// attempt to return early: since both contain unique elements, if they have 
					// different counts, then they can't be equal
					if (mCount != otherAsSet.Count)
					{
						return false;
					}

					// already confirmed that the sets have the same number of distinct elements, so if
					// one is a superset of the other then they must be equal
					return ContainsAllElements(otherAsSet);
				}
				else
				{
					ICollection<TItem> otherAsCollection = other as ICollection<TItem>;
					if (otherAsCollection != null)
					{
						// if this count is 0 but other contains at least one element, they can't be equal
						if (mCount == 0 && otherAsCollection.Count > 0)
						{
							return false;
						}
					}
					//ElementCount result = CheckUniqueAndUnfoundElements(other, true);
					//return (result.uniqueCount == mCount && result.unfoundCount == 0);
					return false;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <param name="array"></param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyTo(TItem[] array) 
			{ 
				CopyTo(array, 0, mCount);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// 
			/// </summary>
			/// <param name="array"></param>
			/// <param name="arrayIndex"></param>
			/// <param name="count"></param>
			//---------------------------------------------------------------------------------------------------------
			public void CopyTo(TItem[] array, Int32 arrayIndex, Int32 count)
			{
				// check array index valid index Int32o array
				if (arrayIndex < 0)
				{
					return;
				}

				// also throw if count less than 0
				if (count < 0)
				{
					return;
				}

				// will array, starting at arrayIndex, be able to hold elements? Note: not
				// checking arrayIndex >= array.Length (consistency with list of allowing
				// count of 0; subsequent check takes care of the rest)
				if (arrayIndex > array.Length || count > array.Length - arrayIndex)
				{
					return;
				}

				Int32 numCopied = 0;
				for (Int32 i = 0; i < mLastIndex && numCopied < count; i++)
				{
					if (mSlots[i].hashCode >= 0)
					{
						array[arrayIndex + numCopied] = mSlots[i].value;
						numCopied++;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Remove elements that match specified predicate. Returns the number of elements removed
			/// </summary>
			/// <param name="match"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 RemoveWhere(Predicate<TItem> match)
			{
				Int32 numRemoved = 0;
				for (Int32 i = 0; i < mLastIndex; i++)
				{
					if (mSlots[i].hashCode >= 0)
					{
						// cache value in case delegate removes it
						TItem value = mSlots[i].value;
						if (match(value))
						{
							// check again that remove actually removed it
							if (Remove(value))
							{
								numRemoved++;
							}
						}
					}
				}
				return numRemoved;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Gets the IEqualityComparer that is used to determine equality of keys for 
			/// the HashSetArray.
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public IEqualityComparer<TItem> Comparer
			{
				get
				{
					return mComparer;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Sets the capacity of this list to the size of the list (rounded up to nearest prime),
			/// unless count is 0, in which case we release references.
			/// 
			/// This method can be used to minimize a list's memory overhead once it is known that no
			/// new elements will be added to the list. To completely clear a list and release all 
			/// memory referenced by the list, execute the following statements:
			/// 
			/// list.Clear();
			/// list.TrimExcess(); 
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void TrimExcess()
			{
				if (mCount == 0)
				{
					// if count is zero, clear references
					mBuckets = null;
					mSlots = null;
					mVersion++;
				}
				else
				{
					// similar to IncreaseCapacity but moves down elements in case add/remove/etc
					// caused fragmentation
					Int32 newSize = XHashHelpers.GetPrime(mCount);
					Slot[] newSlots = new Slot[newSize];
					Int32[] newBuckets = new Int32[newSize];

					// move down slots and rehash at the same time. newIndex keeps track of current 
					// position in newSlots array
					Int32 newIndex = 0;
					for (Int32 i = 0; i < mLastIndex; i++)
					{
						if (mSlots[i].hashCode >= 0)
						{
							newSlots[newIndex] = mSlots[i];

							// rehash
							Int32 bucket = newSlots[newIndex].hashCode % newSize;
							newSlots[newIndex].next = newBuckets[bucket] - 1;
							newBuckets[bucket] = newIndex + 1;

							newIndex++;
						}
					}

					mLastIndex = newIndex;
					mSlots = newSlots;
					mBuckets = newBuckets;
					mFreeList = -1;
				}
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Initializes the HashSetArray from another HashSetArray with the same element type and
			/// equality comparer.
			/// </summary>
			/// <param name="source"></param>
			//---------------------------------------------------------------------------------------------------------
			private void CopyFrom(HashSetArray<TItem> source)
			{
				Int32 count = source.mCount;
				if (count == 0)
				{
					// As well as short-circuiting on the rest of the work done,
					// this avoids errors from trying to access otherAsHashSet.mBuckets
					// or otherAsHashSet.mSlots when they aren't initialized.
					return;
				}

				Int32 capacity = source.mBuckets.Length;
				Int32 threshold = XHashHelpers.ExpandPrime(count + 1);

				if (threshold >= capacity)
				{
					mBuckets = (Int32[])source.mBuckets.Clone();
					mSlots = (Slot[])source.mSlots.Clone();

					mLastIndex = source.mLastIndex;
					mFreeList = source.mFreeList;
				}
				else
				{
					Int32 lastIndex = source.mLastIndex;
					Slot[] slots = source.mSlots;
					Initialize(count);
					Int32 index = 0;
					for (Int32 i = 0; i < lastIndex; ++i)
					{
						Int32 hashCode = slots[i].hashCode;
						if (hashCode >= 0)
						{
							AddValue(index, hashCode, slots[i].value);
							++index;
						}
					}
					mLastIndex = index;
				}
				mCount = count;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Initializes buckets and slots arrays. Uses suggested capacity by finding next prime
			/// greater than or equal to capacity.
			/// </summary>
			/// <param name="capacity"></param>
			//---------------------------------------------------------------------------------------------------------
			private void Initialize(Int32 capacity)
			{
				Int32 size = XHashHelpers.GetPrime(capacity);

				mBuckets = new Int32[size];
				mSlots = new Slot[size];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Expand to new capacity. New capacity is next prime greater than or equal to suggested 
			/// size. This is called when the underlying array is filled. This performs no 
			/// defragmentation, allowing faster execution; note that this is reasonable since 
			/// AddIfNotPresent attempts to insert new elements in re-opened spots.
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void IncreaseCapacity()
			{
				Int32 newSize = XHashHelpers.ExpandPrime(mCount);
				if (newSize <= mCount)
				{

				}

				// Able to increase capacity; copy elements to larger array and rehash
				SetCapacity(newSize, false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Set the underlying buckets array to size newSize and rehash.  Note that newSize
			/// *must* be a prime.  It is very likely that you want to call IncreaseCapacity()
			/// instead of this method.
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void SetCapacity(Int32 newSize, bool forceNewHashCodes)
			{
				Slot[] newSlots = new Slot[newSize];
				if (mSlots != null)
				{
					Array.Copy(mSlots, 0, newSlots, 0, mLastIndex);
				}

				if (forceNewHashCodes)
				{
					for (Int32 i = 0; i < mLastIndex; i++)
					{
						if (newSlots[i].hashCode != -1)
						{
							newSlots[i].hashCode = InternalGetHashCode(newSlots[i].value);
						}
					}
				}

				Int32[] newBuckets = new Int32[newSize];
				for (Int32 i = 0; i < mLastIndex; i++)
				{
					Int32 bucket = newSlots[i].hashCode % newSize;
					newSlots[i].next = newBuckets[bucket] - 1;
					newBuckets[bucket] = i + 1;
				}
				mSlots = newSlots;
				mBuckets = newBuckets;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Adds value to HashSetArray if not contained already
			/// Returns true if added and false if already present
			/// </summary>
			/// <param name="value">value to find</param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private Boolean AddIfNotPresent(in TItem value)
			{
				if (mBuckets == null)
				{
					Initialize(0);
				}

				Int32 hashCode = InternalGetHashCode(value);
				Int32 bucket = hashCode % mBuckets.Length;

				for (Int32 i = mBuckets[hashCode % mBuckets.Length] - 1; i >= 0; i = mSlots[i].next)
				{
					if (mSlots[i].hashCode == hashCode && mComparer.Equals(mSlots[i].value, value))
					{
						return false;
					}
				}

				Int32 index;
				if (mFreeList >= 0)
				{
					index = mFreeList;
					mFreeList = mSlots[index].next;
				}
				else
				{
					if (mLastIndex == mSlots.Length)
					{
						IncreaseCapacity();
						// this will change during resize
						bucket = hashCode % mBuckets.Length;
					}
					index = mLastIndex;
					mLastIndex++;
				}
				mSlots[index].hashCode = hashCode;
				mSlots[index].value = value;
				mSlots[index].next = mBuckets[bucket] - 1;
				mBuckets[bucket] = index + 1;
				mCount++;
				mVersion++;

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Add value at known index with known hash code. Used only
			/// when constructing from another HashSetArray.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="hashCode"></param>
			/// <param name="value"></param>
			//---------------------------------------------------------------------------------------------------------
			private void AddValue(Int32 index, Int32 hashCode, in TItem value)
			{
				Int32 bucket = hashCode % mBuckets.Length;

				mSlots[index].hashCode = hashCode;
				mSlots[index].value = value;
				mSlots[index].next = mBuckets[bucket] - 1;
				mBuckets[bucket] = index + 1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if this contains of other's elements. Iterates over other's elements and 
			/// returns false as soon as it finds an element in other that's not in this.
			/// Used by SupersetOf, ProperSupersetOf, and SetEquals.
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private Boolean ContainsAllElements(IEnumerable<TItem> other)
			{
				foreach (TItem element in other)
				{
					if (!Contains(element))
					{
						return false;
					}
				}
				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Implementation Notes:
			/// If other is a hashset and is using same equality comparer, then checking subset is 
			/// faster. Simply check that each element in this is in other.
			/// 
			/// Note: if other doesn't use same equality comparer, then Contains check is invalid,
			/// which is why callers must take are of this.
			/// 
			/// If callers are concerned about whether this is a proper subset, they take care of that.
			///
			/// </summary>
			/// <param name="other"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private Boolean IsSubsetOfHashSetWithSameEC(HashSetArray<TItem> other)
			{

				foreach (TItem item in this)
				{
					if (!other.Contains(item))
					{
						return false;
					}
				}
				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// If other is a hashset that uses same equality comparer, intersect is much faster 
			/// because we can use other's Contains
			/// </summary>
			/// <param name="other"></param>
			//---------------------------------------------------------------------------------------------------------
			private void IntersectWithHashSetWithSameEC(HashSetArray<TItem> other)
			{
				for (Int32 i = 0; i < mLastIndex; i++)
				{
					if (mSlots[i].hashCode >= 0)
					{
						TItem item = mSlots[i].value;
						if (!other.Contains(item))
						{
							Remove(item);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Used internally by set operations which have to rely on bit array marking. This is like
			/// Contains but returns index in slots array. 
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private Int32 InternalIndexOf(in TItem item)
			{
				Int32 hashCode = InternalGetHashCode(item);
				for (Int32 i = mBuckets[hashCode % mBuckets.Length] - 1; i >= 0; i = mSlots[i].next)
				{
					if ((mSlots[i].hashCode) == hashCode && mComparer.Equals(mSlots[i].value, item))
					{
						return i;
					}
				}
				// wasn't found
				return -1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// if other is a set, we can assume it doesn't have duplicate elements, so use this
			/// technique: if can't remove, then it wasn't present in this set, so add.
			/// 
			/// As with other methods, callers take care of ensuring that other is a hashset using the
			/// same equality comparer.
			/// </summary>
			/// <param name="other"></param>
			//---------------------------------------------------------------------------------------------------------
			private void SymmetricExceptWithUniqueHashSet(HashSetArray<TItem> other)
			{
				foreach (TItem item in other)
				{
					if (!Remove(item))
					{
						AddIfNotPresent(item);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Add if not already in hashset. Returns an out param indicating index where added. This 
			/// is used by SymmetricExcept because it needs to know the following things:
			/// - whether the item was already present in the collection or added from other
			/// - where it's located (if already present, it will get marked for removal, otherwise
			/// marked for keeping)
			/// </summary>
			/// <param name="value"></param>
			/// <param name="location"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private Boolean AddOrGetLocation(in TItem value, out Int32 location)
			{
				Int32 hashCode = InternalGetHashCode(value);
				Int32 bucket = hashCode % mBuckets.Length;
				for (Int32 i = mBuckets[hashCode % mBuckets.Length] - 1; i >= 0; i = mSlots[i].next)
				{
					if (mSlots[i].hashCode == hashCode && mComparer.Equals(mSlots[i].value, value))
					{
						location = i;
						return false; //already present
					}
				}
				Int32 index;
				if (mFreeList >= 0)
				{
					index = mFreeList;
					mFreeList = mSlots[index].next;
				}
				else
				{
					if (mLastIndex == mSlots.Length)
					{
						IncreaseCapacity();
						// this will change during resize
						bucket = hashCode % mBuckets.Length;
					}
					index = mLastIndex;
					mLastIndex++;
				}
				mSlots[index].hashCode = hashCode;
				mSlots[index].value = value;
				mSlots[index].next = mBuckets[bucket] - 1;
				mBuckets[bucket] = index + 1;
				mCount++;
				mVersion++;
				location = index;
				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Copies this to an array. Used for DebugView
			/// </summary>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			internal TItem[] ToArray()
			{
				TItem[] newArray = new TItem[Count];
				CopyTo(newArray);
				return newArray;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Internal method used for HashSetEqualityComparer. Compares set1 and set2 according 
			/// to specified comparer.
			/// 
			/// Because items are hashed according to a specific equality comparer, we have to resort
			/// to n^2 search if they're using different equality comparers.
			/// </summary>
			/// <param name="set1"></param>
			/// <param name="set2"></param>
			/// <param name="comparer"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			internal static Boolean HashSetEquals(HashSetArray<TItem> set1, HashSetArray<TItem> set2, IEqualityComparer<TItem> comparer)
			{
				// handle null cases first
				if (set1 == null)
				{
					return (set2 == null);
				}
				else if (set2 == null)
				{
					// set1 != null
					return false;
				}

				// all comparers are the same; this is faster
				if (AreEqualityComparersEqual(set1, set2))
				{
					if (set1.Count != set2.Count)
					{
						return false;
					}
					// suffices to check subset
					foreach (TItem item in set2)
					{
						if (!set1.Contains(item))
						{
							return false;
						}
					}
					return true;
				}
				else
				{  // n^2 search because items are hashed according to their respective ECs
					foreach (TItem set2Item in set2)
					{
						bool found = false;
						foreach (TItem set1Item in set1)
						{
							if (comparer.Equals(set2Item, set1Item))
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							return false;
						}
					}
					return true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Checks if equality comparers are equal. This is used for algorithms that can
			/// speed up if it knows the other item has unique elements. I.e. if they're using 
			/// different equality comparers, then uniqueness assumption between sets break.
			/// </summary>
			/// <param name="set1"></param>
			/// <param name="set2"></param>
			/// <returns></returns>
			//---------------------------------------------------------------------------------------------------------
			private static Boolean AreEqualityComparersEqual(HashSetArray<TItem> set1, HashSetArray<TItem> set2)
			{
				return set1.Comparer.Equals(set2.Comparer);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Workaround Comparers that throw ArgumentNullException for GetHashCode(null).
			/// </summary>
			/// <param name="item"></param>
			/// <returns>hash code</returns>
			//---------------------------------------------------------------------------------------------------------
			private Int32 InternalGetHashCode(in TItem item)
			{
				if (item == null)
				{
					return 0;
				}
				return mComparer.GetHashCode(item) & Lower31BitMask;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================