[recompile(fix package)] https://github.com/NinevaStudios/mqtt-utilities-unreal


mosquitto （依赖openssl）使用
	mosquitto.exe 启动代理服务器（broker）？或 test.mosquitto.org 测试服务器（topic -t“#”监听所有）
	mosquitto_sub.exe 注册监听  mosquitto_sub.exe -h test.mosquitto.org -t "#"
		 -h : mqtt host to connect to. Defaults to localhost. 【服务器地址ip】
		 -i : id to use for this client. Defaults to mosquitto_sub_ appended with the process id.
		 -I : define the client id as id_prefix appended with the process id. Useful for when the
		      broker is using the clientid_prefixes option.
 		-k : keep alive in seconds for this client. Defaults to 60.
 		-L : specify user, password, hostname, port and topic as a URL in the form:
 		     mqtt(s)://[username[:password]@]host[:port]/topic
 		-N : do not add an end of line character when printing the payload.
 		-p : network port to connect to. Defaults to 1883 for plain MQTT and 8883 for MQTT over TLS. 【端口】
 		-P : provide a password 【密码】
 		-q : quality of service level to use for the subscription. Defaults to 0.
 		-R : do not print stale messages (those with retain set).
 		-t : mqtt topic to subscribe to. May be repeated multiple times. 【话题】
 		-T : topic string to filter out of results. May be repeated. 
 		-u : provide a username 【用户名】
		-U : unsubscribe from a topic. May be repeated.
 		-v : print published messages verbosely.
	mosquitto_pub.exe 发布信息  mosquitto_pub.exe -h test.mosquitto.org -t TestTopic  -m "Hello" 