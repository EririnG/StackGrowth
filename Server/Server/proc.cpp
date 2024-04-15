#include <iostream>
using namespace std;

void login_proc(char* buf)
{
	char* id = NULL;
	char* pw = NULL;
	id = strtok_s(buf, "/", &pw);

	cout << "Recv ID : " << id << endl;
	cout << "Recv PW : " << pw << endl;
}
