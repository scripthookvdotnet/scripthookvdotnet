#include "DependencyGraph.hpp";

namespace GTA {
	DependencyGraph::Dependency::Dependency(String^ name)
	{
		mName = name;
		mDependencies = gcnew List<Dependency^>();
		mDependants = gcnew List<Dependency^>();
	}

	int DependencyGraph::Dependency::CompareTo(Object^ other)
	{
		if (dynamic_cast<Dependency^>(other) == nullptr) {
			throw gcnew ArgumentException("Object is not a Dependency");
			return 0;
		}
		Dependency^ d = dynamic_cast<Dependency^>(other);
		if (this->Dependencies->Count < d->Dependencies->Count) return -1;
		if (this->Dependencies->Count > d->Dependencies->Count) return 1;
		return 0;
	}

	DependencyGraph::DependencyGraph()
	{
		dependencyList = gcnew Dictionary<String^, Dependency^>();
	}

	void DependencyGraph::AddScript(String^ s)
	{
		if (!dependencyList->ContainsKey(s)) dependencyList->Add(s, gcnew Dependency(s));
	}

	void DependencyGraph::LinkDependency(String^ dependent, String^ dependsOn)
	{
		AddScript(dependent);
		AddScript(dependsOn);
		dependencyList[dependent]->Dependencies->Add(dependencyList[dependsOn]);
		dependencyList[dependsOn]->Dependants->Add(dependencyList[dependent]);
	}

	List<DependencyGraph::Dependency^>^ DependencyGraph::SortDependencies()
	{
		List<Dependency^>^ L = gcnew List<Dependency^>();
		List<Dependency^>^ S = gcnew List<Dependency^>();
		for each (auto d in dependencyList->Values)
		{
			if (d->Dependencies->Count == 0) S->Add(d);
		}

		while (S->Count > 0)
		{
			S->Sort();
			Dependency^ d = S[0];
			S->Remove(d);

			if (!L->Contains(d)) L->Add(d);

			for each(auto d in d->Dependants)
			{
				if (!L->Contains(d)) S->Add(d);
			}
		}

		return L;
	}
}