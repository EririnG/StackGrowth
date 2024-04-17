#include<WinSock2.h>
#include<cstdlib> 
#include<ctime> 

void err_quit(const char* msg);
void err_display(const char* msg);
void packet_proc(SOCKET sock, char* p_packet);
void login_proc(SOCKET cli_sock, char* buf);
void vaildate_log_id(SOCKET cli_sock, char* id);
void vaildate_log_pw(SOCKET cli_sock, char* id, char* pw);
void register_proc(SOCKET cli_sock, char* buf);
void vaildate_nickname(SOCKET cli_sock, char* nick);
void vaildate_id(SOCKET cli_sock, char* id);
void post_proc(SOCKET cli_sock, char* buf);
void open_proc(SOCKET cli_sock);
void make_proc(SOCKET cli_sock, char* buf);
void p_proc(SOCKET sock, char* buf);