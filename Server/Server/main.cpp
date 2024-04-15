#pragma once
#pragma comment(lib,"ws2_32.lib")   // Window소켓 라이브러리 링크
//#pragma comment(lib,"winmm.lib")    // Window 멀티미디어 관련 API 포함

#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define BUFSIZE 1024
#define SERVERPORT 50001

#include <winsock2.h>
#include <Ws2tcpip.h>
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <string.h>
#include <thread>
#include "packet.h"
#include <cstring>



using namespace std;

// 소켓 관련 전역 변수
static SOCKET listen_sock;

// 서버를 죽일 때 사용하는 플래그
static bool dead_flag = false;

// 총 소켓 수를 체크하는 변수
int total_sockets = 0;



// 소켓 함수 오류 출력 후 종류
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

// 소켓 함수 오류 출력
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



int main(int argc, char* argv[])
{
	int retval;

	// 윈속 초기화
	WSADATA wsa;
	if (WSAStartup(MAKEWORD(2, 2), &wsa) != 0)
		return 1;

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
	retval = bind(listen_sock, (SOCKADDR*)&serv_addr, sizeof(serv_addr));
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
		// 접속한 클라이언트 정보 출력
		char cli_ip[33] = { 0, };
		inet_ntop(AF_INET, &(cli_addr.sin_addr), cli_ip, 33 - 1);
		printf("\n[TCP 서버] 클라이언트 접속 : IP 주소 = %s, 포트번호 : %d\n", cli_ip, ntohs(cli_addr.sin_port));

		// 클라이언트와 데이터 통신
		char buf[BUFSIZE + 1];
		
		while (1)
		{
			int recv_size = recv(cli_sock, buf, BUFSIZE, 0);
			if(recv_size > 0)
				buf[recv_size] = '\0';
			
			char* id;
			char* pw = NULL;
			id=  strtok_s(buf, "/", &pw);
			cout << "Recv ID : " << id << endl;
			cout << "Recv PW : " << pw << endl;


			cout << "Recv Data : " << buf << endl;
			cout << "Recv Len : " << recv_size << endl;
			int result_code = send(cli_sock, buf, recv_size, 0);
			
			if (recv_size == SOCKET_ERROR)
			{
				err_display("Recv()");
				break;
			}
			printf("[recv : %d]\n", recv_size);

			int read_pos = 0;
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
		printf("[TCP 서버] 클라이언트 종류 : IP 주소 : %s, 포트 번호 : %d\n", cli_ip, ntohs(cli_addr.sin_port));
	}

	closesocket(listen_sock);
	WSACleanup();
	return 0;
}
