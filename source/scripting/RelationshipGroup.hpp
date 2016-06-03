#pragma once

namespace GTA
{
	#pragma region Forward Declarations
	enum class Relationship;
	#pragma endregion

	public value class RelationshipGroup : System::IEquatable<RelationshipGroup>
	{
	public:
		RelationshipGroup(int hash);
		RelationshipGroup(unsigned int hash);
		
		property int Hash
		{
			int get();
		}

		Relationship GetRelationshipBetweenGroups(RelationshipGroup targetGroup);
		void SetRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship);
		void SetRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool isBidirectional);
		void ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship);
		void ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool isBidirectional);

		void Remove();

		virtual bool Equals(System::Object ^obj) override;
		virtual bool Equals(RelationshipGroup model);

		virtual inline int GetHashCode() override
		{
			return Hash;
		}
		virtual inline System::String ^ToString() override
		{
			return "0x" + static_cast<System::UInt32>(Hash).ToString("X");
		}

		static inline operator RelationshipGroup(int source)
		{
			return RelationshipGroup(source);
		}
		static inline operator RelationshipGroup(unsigned int source)
		{
			return RelationshipGroup(source);
		}
		static inline operator RelationshipGroup(System::String ^source)
		{
			return RelationshipGroup(source);
		}

		static inline bool operator==(RelationshipGroup left, RelationshipGroup right)
		{
			return left.Equals(right);
		}
		static inline bool operator!=(RelationshipGroup left, RelationshipGroup right)
		{
			return !operator==(left, right);
		}
	private:
		int _hash;
		RelationshipGroup(System::String ^name);
	};
}