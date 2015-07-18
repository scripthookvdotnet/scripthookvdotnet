using namespace System;
using namespace System::Collections::Generic;

namespace GTA {
	public ref class DependencyGraph {
	public:
		DependencyGraph();

		ref class Dependency : IComparable {
		public:
			Dependency(String^ name);
			virtual Int32 CompareTo(Object^ other);

			property String^ Name{
				String ^get()
				{
					return this->mName;
				}
			}

			property List<Dependency^>^ Dependencies{
				List<Dependency^>^ get()
				{
					return this->mDependencies;
				}
			}

			property List<Dependency^>^ Dependants {
				List<Dependency^>^ get()
				{
					return this->mDependants;
				}
			}

		private:
			String^ mName;
			List<Dependency^>^ mDependencies;
			List<Dependency^>^ mDependants;
		};

		void AddScript(String^ s);
		void LinkDependency(String^ d1, String^ d2);
		List<Dependency^>^ SortDependencies();

	private:
		Dictionary<String^, Dependency^>^ dependencyList;
	};
}