#include <fstream>
#include <string>
#include <vector>
#include <iterator>

using namespace std;

string trim(string& str)
{
	int start = str.find_first_not_of(' ');
	int len = str.find_last_not_of(' ')+1 - start;
	return str.substr(start, len);
}

int main ( int argc, char *argv[] )
{
	if (argc != 3)
	{
		return 0;
	}
	ifstream source(argv[1]);
	ifstream source2(argv[2]);
	if (!source)
	{
		return 0;
	}if (!source2)
	{
		return 0;
	}
	vector<string> Lines;
	string curLine;
	while (getline(source, curLine))
	{
		if (trim(curLine) == "</members>")
		{
			bool write = false;
			while (getline(source2, curLine))
			{
				if (trim(curLine) == "<members>")
				{
					write = true;
					continue;
				}
				if (trim(curLine) == "</members>")
					break;
				if (write)
				{
					Lines.push_back(curLine);
				}
			}
		}
		Lines.push_back(curLine);
	}
	source.close();
	source2.close();
	ofstream output(argv[1]);//need to give a proper name here
	ostream_iterator<string> output_iterator(output, "\n");
    copy(Lines.begin(), Lines.end(), output_iterator);
	output.close();
	return 0;	
}