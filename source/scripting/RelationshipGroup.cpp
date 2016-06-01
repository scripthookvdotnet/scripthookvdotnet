#include "RelationshipGroup.hpp"
#include "Native.hpp"
#include "World.hpp"

namespace GTA
{
	RelationshipGroup::RelationshipGroup(System::String ^name)
	{
		int hash = 0;
		Native::Function::Call(Native::Hash::ADD_RELATIONSHIP_GROUP, name, &hash);

		_hash = hash;
	}
	RelationshipGroup::RelationshipGroup(int hash) : _hash(hash)
	{
	}
	RelationshipGroup::RelationshipGroup(unsigned int hash) : _hash(static_cast<int>(hash))
	{
	}

	int RelationshipGroup::Hash::get()
	{
		return _hash;
	}

	Relationship RelationshipGroup::GetRelationshipBetweenGroups(RelationshipGroup targetGroup)
	{
		return static_cast<Relationship>(Native::Function::Call<int>(Native::Hash::GET_RELATIONSHIP_BETWEEN_GROUPS, Hash, targetGroup));
	}
	void RelationshipGroup::SetRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship)
	{
		SetRelationshipBetweenGroups(targetGroup, relationship, false);
	}
	void RelationshipGroup::SetRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool isBidirectional)
	{
		Native::Function::Call(Native::Hash::SET_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), Hash, targetGroup);
		if (isBidirectional)
		{
			Native::Function::Call(Native::Hash::SET_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), targetGroup, Hash);
		}
	}
	void RelationshipGroup::ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship)
	{
		ClearRelationshipBetweenGroups(targetGroup, relationship, false);
	}
	void RelationshipGroup::ClearRelationshipBetweenGroups(RelationshipGroup targetGroup, Relationship relationship, bool isBidirectional)
	{
		Native::Function::Call(Native::Hash::CLEAR_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), Hash, targetGroup);
		if (isBidirectional)
		{
			Native::Function::Call(Native::Hash::CLEAR_RELATIONSHIP_BETWEEN_GROUPS, static_cast<int>(relationship), targetGroup, Hash);
		}
	}

	void RelationshipGroup::Remove()
	{
		Native::Function::Call(Native::Hash::REMOVE_RELATIONSHIP_GROUP, Hash);
	}

	bool RelationshipGroup::Equals(Object ^value)
	{
		if (value == nullptr || value->GetType() != GetType())
		{
			return false;
		}

		return Equals(safe_cast<RelationshipGroup>(value));
	}
	bool RelationshipGroup::Equals(RelationshipGroup relationShipGroup)
	{
		return Hash == relationShipGroup.Hash;
	}
}