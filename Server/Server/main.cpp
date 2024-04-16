#pragma once
#pragma comment(lib,"ws2_32.lib")   // Window���� ���̺귯�� ��ũ
//#pragma comment(lib,"winmm.lib")    // Window ��Ƽ�̵�� ���� API ����

#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define BUFSIZE 1024
#define SERVERPORT 50002

#include <winsock2.h>
#include <Ws2tcpip.h>
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <string.h>
#include <thread>
#include <cstring>
#include <mysql.h>

#include "packet.h"
#include "main.h"

using namespace std;


struct DBConnection
{
	const char* server = "localhost",
		* user = "root",
		* pw = "1234",
		* db = "erin_db";
}db_conn;

MYSQL mysql;

// ���� ���� ���� ����
static SOCKET listen_sock;

// ������ ���� �� ����ϴ� �÷���
static bool dead_flag = false;

// �� ���� ���� üũ�ϴ� ����
int total_sockets = 0;



// ���� �Լ� ���� ��� �� ����
void err_quit(const char* msg)
{
	LPVOID lp_msg_buf;
	FormatMessage(
		FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(),
		MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lp_msg_buf, 0, NULL);
	MessageBox(NULL, (LPCTSTR)lp_msg_buf, msg, MB_ICONERROR);
	LocalFree(lp_msg_buf);
	exit(1);
}

// ���� �Լ� ���� ���
void err_display(const char* msg)
{
	LPVOID lp_msg_buf;
	FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER | FORMAT_MESSAGE_FROM_SYSTEM,
		NULL, WSAGetLastError(), MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
		(LPTSTR)&lp_msg_buf, 0, NULL);
	printf("[%s] %s", msg, (char*)lp_msg_buf);
	LocalFree(lp_msg_buf);

}

void packet_proc(SOCKET sock, char* p_packet)
{
	PktRes res;
	res.pack_id = (short)PACKET_ID::RES;
	res.total_size = (short)sizeof(PktRes);
	res.num = 0;

	PktHeader* p_header = (PktHeader*)p_packet;

	if (p_header->pack_id == (short)PACKET_ID::REGISTER)
	{
		
	}
	else if (p_header->pack_id == (short)PACKET_ID::LOGIN)
	{

	}
}

void login_proc(SOCKET cli_sock ,char* buf)
{
	char* id = NULL;
	char* pw = NULL;
	id = strtok_s(buf, "/", &pw);

	cout << "Recv ID : " << id << endl;
	cout << "Recv PW : " << pw << endl;
}

void register_proc(SOCKET cli_sock, char* buf)
{
	char* nick = NULL;
	char* id = NULL;
	char* pw = NULL;
	char query[1024];
	
	nick = strtok_s(buf, "/", &id);
	cout << "nick : " << nick << endl;
	cout << "���������� : " << id << endl;
	id = strtok_s(id, "/", &pw);
	cout << "id : " << id << endl;
	cout << "pw : " << pw << endl;


	vaildate_nickname(cli_sock,nick, id);

	/*strcpy(query, "INSERT INTO 'erin_db' ('name','id','pw') VALUSE ('%s','%s','%s')", nick, id, pw);*/
	sprintf_s(query, "INSERT INTO `erin_db`.`user_info` (`name`,`id`,`pw`) VALUES ('%s','%s','%s')", nick, id, pw);
		
	if (!mysql_query(&mysql, query))
	{
		cout << "ȸ������ ����" << endl;
	}
	else
	{
		cout << "ȸ������ ����\n���� ���� :"<< mysql_error(&mysql) << endl;
	}
}

void vaildate_nickname(SOCKET cli_sock, char* nick, char* id)
{
	char query[1024];
	int rowCount;
	MYSQL_RES* result;
	MYSQL_ROW row;
	
	// �г��� �ߺ� �˻�
	sprintf_s(query, "SELECT COUNT(*) FROM erin_db.user_info WHERE name = '%s'", nick);
	if (!mysql_query(&mysql, query))
	{
		cout << "�г��� �˻� ����" << endl;
	}
	else
	{
		cout << "�г��� �˻� ����\n���� ���� :" << mysql_error(&mysql) << endl;
	}	
	result = mysql_store_result(&mysql);
	row = mysql_fetch_row(result);
	rowCount = std::stoi(row[0]);
	cout << rowCount << endl;
	if (rowCount > 0)
	{
		send(cli_sock, "1", 1, 0);
	}
	exit(1);
}

void vaildate_id(SOCKET cli_sock, char* nick, char* id)
{
	char query[1024];
	int rowCount;
	MYSQL_RES* result;
	MYSQL_ROW row;

	// �г��� �ߺ� �˻�
	sprintf_s(query, "SELECT COUNT(*) FROM erin_db.user_info WHERE name = '%s'", nick);
	if (!mysql_query(&mysql, query))
	{
		cout << "�г��� �˻� ����" << endl;
	}
	else
	{
		cout << "�г��� �˻� ����\n���� ���� :" << mysql_error(&mysql) << endl;
	}
	result = mysql_store_result(&mysql);
	row = mysql_fetch_row(result);
	rowCount = std::stoi(row[0]);
	cout << rowCount << endl;
	if (rowCount > 0)
	{
		send(cli_sock, "1", 1, 0);
	}
	exit(1);
}


void p_proc(SOCKET cli_sock, char* buf)
{
	char* p_id;
	char* data = NULL;
	p_id = strtok_s(buf, "/", &data);



	switch (stoi(p_id))
	{
	case static_cast<int>(PACKET_ID::REGISTER):
		cout << "�ϴ� ����� ��" << endl;
		register_proc(cli_sock,data);
		break;

	case static_cast<int>(PACKET_ID::LOGIN):
		login_proc(cli_sock,data);
		break;

	default:
		break;
	}

}



int main(int argc, char* argv[])
{

	int retval;

	// ���� �ʱ�ȭ
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		return 1;
	// mysql ����
	mysql_init(&mysql);
	if (mysql_real_connect(&mysql, db_conn.server, db_conn.user, db_conn.pw, db_conn.db, 50001, NULL, 0))
	{
		cout << "MySQL ���� : " << mysql_get_client_info() << endl;
		mysql_set_character_set(&mysql, "utf8");
	}
	else
		cout << "MySQL ���� ���� \n���� ���� : "<< mysql_error(&mysql) << endl;

	// sock()
	listen_sock = socket(AF_INET, SOCK_STREAM, 0);
	if (listen_sock == INVALID_SOCKET)
		err_quit("Socket()");
	// bind()
	SOCKADDR_IN serv_addr;
	ZeroMemory(&serv_addr, sizeof(serv_addr));
	serv_addr.sin_family = AF_INET;
	serv_addr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_addr.sin_port = htons(SERVERPORT);
	retval = ::bind(listen_sock, (SOCKADDR*)&serv_addr, sizeof(serv_addr));
	if (retval == SOCKET_ERROR)
		err_quit("Bind()");
	// listen()
	retval = listen(listen_sock, SOMAXCONN);
	if (retval == SOCKET_ERROR)
		err_quit("Listen()");
	while (1)
	{
		SOCKADDR_IN cli_addr;
		int addr_len = sizeof(cli_addr);

		SOCKET cli_sock = accept(listen_sock, (SOCKADDR*)&cli_addr, &addr_len);
		if (cli_sock == INVALID_SOCKET)
		{
			err_display("Accept()");
			break;
		}
		// ������ Ŭ���̾�Ʈ ���� ���
		char cli_ip[33] = { 0, };
		inet_ntop(AF_INET, &(cli_addr.sin_addr), cli_ip, 33 - 1);
		printf("\n[TCP ����] Ŭ���̾�Ʈ ���� : IP �ּ� = %s, ��Ʈ��ȣ : %d\n", cli_ip, ntohs(cli_addr.sin_port));

		// Ŭ���̾�Ʈ�� ������ ���
		char buf[BUFSIZE + 1];
		
		while (1)
		{
			int recv_size = recv(cli_sock, buf, BUFSIZE, 0);
			string msg = buf;
			if (recv_size == SOCKET_ERROR)
			{
				err_display("Recv()");
				break;
			}
			if(recv_size > 0)
				buf[recv_size] = '\0';

			printf("[recv : %d]\n", recv_size);

			p_proc(cli_sock, buf);

		/*	char* p_id;
			char* data = NULL;
			p_id =  strtok_s(buf, "/", &data);
			

			char* id = NULL;
			char* pw = NULL;

			cout << "Recv ID : " << id << endl;
			cout << "Recv PW : " << pw << endl;

			cout << "Recv Data : " << buf << endl;
			cout << "Recv Len : " << recv_size << endl;
			int result_code = send(cli_sock, buf, recv_size, 0);*/
			

			/*while (recv_size >= PACKET_HEADER_SIZE)
			{
				PktHeader* p_header = (PktHeader*)&buf[read_pos];
				if (p_header->total_size > recv_size)
					break;
				packet_proc(cli_sock, &buf[read_pos]);
				read_pos += p_header->total_size;
				recv_size -= p_header->total_size;
			}*/
		}

		closesocket(cli_sock);
		printf("[TCP ����] Ŭ���̾�Ʈ ���� : IP �ּ� : %s, ��Ʈ ��ȣ : %d\n", cli_ip, ntohs(cli_addr.sin_port));
	}

	closesocket(listen_sock);
	mysql_close(&mysql);
	WSACleanup();
	return 0;
}
