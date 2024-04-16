#pragma once

enum class PACKET_ID : short
{
	REGISTER = 21,
	LOGIN = 22,
	POST = 23,
	OPENPOST = 24,
	MAKEPOST = 25,

	RES = 31,
};

#pragma pack(push,1)
const int PACKET_HEADER_SIZE = 5;
struct PktHeader
{
	short total_size;
	short pack_id;
	unsigned char reserve;
};

struct PktCalcu2Req
{
	int n1;
	short op1;
	int n2;	
};

struct PktCalcu3Req
{
	int n1;
	short op1;
	int n2;
	short op2;
	int n3;
};

struct PktRes : PktHeader
{
	int num;
};
#pragma pack(pop)