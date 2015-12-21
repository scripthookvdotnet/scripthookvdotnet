#pragma once
#pragma managed

namespace GTA 
{
	#pragma region Forward Declarations
	ref class Script;
	#pragma endregion

	public ref class ScriptChild abstract 
	{
	internal:
		ScriptChild();

	protected:
		property Script^ Parent
		{ 
			Script^ get();
		}	

	private:
		Script^ _parent;
	};
}