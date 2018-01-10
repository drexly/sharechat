# sharechat

## DEVISED IDEA : Non Socket Programmed ShareFolder Based (Featured AES256 conversation Encrypt/Decrypt by SSK) Portable Chatting App

1. Multi-Users executing the "sharechat" in the same shared folder concurrently via INTRANET(also works without internet), Dialogue perfectly secured by preset password's encryption for each user input.

2. No server, No client, and No mercy for not knowing the password. Any wrong login attempt will be notified to the others in chatroom, with logging the typed wrong passwords and the IP address of the intruder on the Encrypted chatlog.

3. Without a certification key file or a password, it's Non-admissible and should start over the key making process with empty chatlog 

4. If any user needs to instant clean-up with leaving no evidence including using of the chatapp, call "자비스, 메리크리스마스(Jarvis, merry christmas)" with typing password for next input. It all terminates every app process instantly from world-wide chat-app users.

### Since the "TERMINATE" ChatBot command is written inside the encrypted sharing chatlog, it's impossible to start the app again without deleting the previous chatlog("said.log"). Same password will be retained while not deleting the previously created SSK("rkey.cer")

and

DEFAULT PASSWORD IS password

* * *

챗봇을 쓰려면 자비스, 를 부르세요(호출시 비밀번호 필요)

자비스, 메리크리스마스를 명령하시면 해당 암호화 채팅을 공유하는 전 세계 sharechat의 클라이언트 채팅 내역이 삭제되고 해당 모든 컴퓨터들에서 즉시 종료됩니다.

(기존 암호화 대화 로그-said.log 삭제 전에는 앱 재실행 불가)

* * *

## System introduction for Actual use

### Main UI describing the usage of app function keys.

(e.g. Secure mode switch:F6, Windows Start up program registry registration:F10...)

In "BALLOON" mode the main app UI flickers when any new change turns out, while "POP" mode throws notifications from the right-corner taskbar.

![https://github.com/drexly/sharechat](/intro/a.png?raw=true "Main UI")

* * *
### Type password for initial use when start from the scratch, or login with predefined password + created Symmetric Single Key pair.

![https://github.com/drexly/sharechat](/intro/a1.png?raw=true "Main UI")

* * *
### Start from the scratch: Modal for creating initial password and its Secret Symmetric Single Key (signed by password).

The First field for admin password login, granting an authority of making a new password of its app.

(pre-defined in the source code to make a customizable release for each app in the compiling stage.)

The Second field will be activated by successful login of the First field, so create new password thereafter by writing on the Second field.

After making a new password, the modal will be closed and should write that password exactly on the main UI dialogue textInput.

With the successful MainUI login, a new password key signed Secret Symmetric Single Key file("rkey.cer") and conversation dialogue("said.log") written in AES256 encrypted by the key will be created in the current app executing folder.

![https://github.com/drexly/sharechat](/intro/a2.png?raw=true "Main UI")

* * *
### Thereby starts ShareChat in the SECURE MODE which hides the contents and turns into the previous login mode when outfocused(clicking another app) by the user.

Contents are AES256 encrypted with the key and only decryptable by the pre-authorized MainUI.

If a wrong password is tried for logging in, the system alerts the wrong password in the contents and kicks out that user by exiting the app of the user.

For using multiple chat apps simultaneously for each person, the app's name is consisted of current app running folder's name

(e.g. Since below screenshot's running folder is named as sharchat, the app's notification title is the "sharchat")

![https://github.com/drexly/sharechat](/intro/a3.png?raw=true "System Initialization")

* * *
### The app UI support transparent mode calibrating with F1/F2 key, also supporting inverted color mode by toggling F3. Initial launch starts with SECURE MODE, which can be turned on/off by F6.

![https://github.com/drexly/sharechat](/intro/a35.png?raw=true "other person's chat logging in")
![https://github.com/drexly/sharechat](/intro/a4.png?raw=true "Starting chat with others")

* * *
### While running the chatapp, you can minmize it to windows' taskbar by clicking close. You can "Real Exit" or read the "Help" by right-clicking its minimized icon in the taskbar.

![https://github.com/drexly/sharechat](/intro/b.png?raw=true "Notification of Minimize ")
![https://github.com/drexly/sharechat](/intro/c.png?raw=true "right-clicking its minimized icon in the taskbar")
![https://github.com/drexly/sharechat](/intro/d.png?raw=true "Clicking Help Key")

* * *
### Due to the existence of Secure mode, New message's Notification and its contents are also secured when it's locked by SECURE MODE.

![https://github.com/drexly/sharechat](/intro/a36.png?raw=true "Secure Mode on")

* * *
### By locking off the secure mode with F6 while logging in, New message's Notification and its contents can be shown directly to the user.

![https://github.com/drexly/sharechat](/intro/a37.png?raw=true "Secure Mode off")

* * *
### The system ChatBot AI:Jarvis will be activated by logging in the correct password. Below image explicates the error notification of calling Jarvis with wrong password.

![https://github.com/drexly/sharechat](/intro/a38.png?raw=true "JARVIS gone wrong")

* * *
### For instant chat clean-up with leaving no evidence including using of the chatapp, call "자비스, 메리크리스마스(Jarvis, merry christmas)" with typing password for next input. The system ChatBot AI:Jarvis will terminate all the chatapp processes running world-wide, simultaneously.

![https://github.com/drexly/sharechat](/intro/a39.png?raw=true "JARVIS command call")
![https://github.com/drexly/sharechat](/intro/a40.png?raw=true "JARVIS password input")
![https://github.com/drexly/sharechat](/intro/a41.png?raw=true "JARVIS kicks out")

* * *
### After "자비스, 메리크리스마스(Jarvis, merry christmas)" termination, Neither reading/decrypting the existing chatlog nor running the chat-app again is NOT possible. Should delete existing chatlog("said.log") to start over.

![https://github.com/drexly/sharechat](/intro/a42.png?raw=true "JARVIS kicks out")

