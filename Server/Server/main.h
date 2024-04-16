#include<WinSock2.h>

void err_quit(const char* msg);
void err_display(const char* msg);
void packet_proc(SOCKET sock, char* p_packet);
void login_proc(SOCKET cli_sock, char* buf);
void register_proc(SOCKET cli_sock,char* buf);
void vaildate_register(SOCKET cli_sock, char* nick, char* id);
void p_proc(SOCKET cli_sock, char* buf);