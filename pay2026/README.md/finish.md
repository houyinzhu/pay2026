# 本项目涵盖的 Python 知识点（汇总版）

## 基础语法与数据类型
- 变量/赋值、字符串拼接与格式化（含 f-string）。
- 基本类型：str/int/float/bool/None；容器：list/dict/tuple/set。
- 常用操作：切片、遍历、推导式（列表/字典）、类型转换。

## 控制流与异常
- 条件分支 if/elif/else；循环 for/while；break/continue。
- 异常处理 try/except/else/finally，raise 自定义异常。

## 函数与面向对象
- 函数定义/参数（默认、可变、关键字）/返回值/文档字符串。
- 内置函数：len/range/enumerate/zip/isinstance 等。
- 类与对象：__init__、实例属性/方法，封装与简单多态。

## 模块、包与文件操作
- import / from...import / 别名导入 / 条件导入。
- 文件读写（with 上下文、编码）、路径与目录操作（os.path）。
- JSON 序列化/反序列化（json.load/dump/loads/dumps）。

## 网络与并发
- HTTP 请求：requests（GET/POST/PUT，headers、timeout、状态码检查）。
- WebSocket：websocket.WebSocketApp，on_message/on_error/on_close/on_open。
- 简单多线程：_thread 启动线程，轮询标志位。

## 第三方库实践
- pygame：窗口、事件循环、绘制、字体、音频、计时器。
- streamlit：页面配置、sidebar、会话状态、表单/聊天组件、rerun/stop。
- requests：API 调用、JSON 负载、错误处理。
- websocket：TTS 实时流式接收与文件写入。

## 加密与时间
- hashlib/hmac/base64：签名与编码；urllib.parse：URL 处理。
- datetime/time：时间戳、格式化、sleep。

## 项目特定模式
- 状态机：游戏状态（MENU/PLAYING/CRASHED 等）。
- 会话与记忆：JSON 持久化（load/save），角色提示词与退出规则。
- Prompt 构建：依据速度/车型动态生成描述，驱动图生图 API。
- 异步与协程风格：Unity 侧用协程，Python 侧用轮询/回调。

## 最佳实践（贯穿各文件）
- 日志与错误提示；输入校验与回退默认值。
- 模块化拆分：api/roles/memory/logic/chat/streamlit_app 等。
- 配置与密钥：环境/文件读取，留出默认值与异常兜底。

（来源文件示例：101.py、glm.py、000work.py、memory-refatored/ 下 api.py、main.py、memory.py、roles.py、chat.py、streamlit_app.py、xunfei_tts.py 等。）
三元表达式：color = self.hover_color if self.is_hovered else self.color
成员运算符：in, not in
比较运算符：==, !=, <, >, <=, >=
逻辑运算符：and, or, not
3.2 循环语句
for循环：
遍历列表：for cloud in self.clouds:
遍历字典：for key, button in self.speed_buttons.items():
范围循环：for i in range(10):
enumerate：for i, instruction in enumerate(instructions):
while循环：
  while not tts_complete and (time.time() - start_time) < timeout:      time.sleep(0.1)
循环控制：
break - 跳出循环
continue - 跳过本次迭代
pass - 占位符
3.3 异常处理
try-except：
  try:      response = requests.post(url, headers=headers, json=data)      return response.json()  except Exception as e:      print(f"发生错误: {e}")
try-except-else-finally：完整的异常处理结构
异常类型：Exception, ValueError, KeyError, FileNotFoundError
抛出异常：raise Exception(f"API调用失败: {response.status_code}")
自定义异常：创建异常类
四、函数
4.1 函数定义与调用
定义函数：
  def call_zhipu_api(messages, model="glm-4-flash"):      # 函数体      return response.json()
函数调用：result = call_zhipu_api(messages)
参数类型：
位置参数
关键字参数
默认参数：def init_font(size=30):
可变参数：*args, **kwargs
4.2 函数特性
返回值：return 语句，可返回多个值
文档字符串：函数说明文档
lambda函数：匿名函数
嵌套函数：函数内部定义函数
闭包：函数返回函数
装饰器：函数装饰器（项目中未直接使用，但streamlit有类似机制）
4.3 内置函数
print(), input(), len(), type(), isinstance()
str(), int(), float(), bool()
range(), enumerate(), zip()
min(), max(), sum(), abs()
open(), file() - 文件操作
dir(), help() - 内省函数
五、类和对象
5.1 类定义
类定义：
  class Button:      def __init__(self, x, y, width, height, text, color):          self.rect = pygame.Rect(x, y, width, height)          self.text = text
类属性：类变量
实例属性：self.attribute
特殊方法：__init__(), __str__(), __repr__()
5.2 方法
实例方法：def draw(self, surface):
类方法：@classmethod
静态方法：@staticmethod
方法调用：button.draw(screen)
5.3 面向对象特性
封装：将数据和方法封装在类中
继承：类继承（项目中较少使用）
多态：不同对象对同一方法的不同实现
属性访问：self.attribute, object.method()
六、模块和包
6.1 导入模块
import语句：
  import requests  import json  import pygame  import os
from...import：
  from memory import load_memory, save_memory  from roles import get_role_prompt  from datetime import datetime
导入别名：import _thread as thread
条件导入：
  try:      import streamlit as st      ZHIPU_API_KEY = st.secrets.get("ZHIPU_API_KEY", "default")  except:      ZHIPU_API_KEY = "default"
6.2 模块使用
标准库模块：
os - 操作系统接口
sys - 系统相关参数和函数
json - JSON编码解码器
time - 时间相关函数
datetime - 日期时间处理
math - 数学函数
random - 随机数生成
platform - 平台识别
ssl - SSL/TLS支持
hashlib - 哈希算法
base64 - Base64编码
hmac - HMAC消息认证
urllib.parse - URL解析
wsgiref.handlers - WSGI工具
第三方库模块：
requests - HTTP库
pygame - 游戏开发库
streamlit - Web应用框架
websocket - WebSocket客户端
6.3 包结构
包导入：from memory_refatored import api
相对导入：from . import module
init_.py：包初始化文件
七、文件操作
7.1 文件读写
打开文件：
  with open(MEMORY_FILE, 'r', encoding='utf-8') as f:      data = json.load(f)
文件模式：'r'（读）, 'w'（写）, 'a'（追加）, 'rb'（二进制读）, 'wb'（二进制写）
上下文管理器：with 语句自动关闭文件
文件编码：encoding='utf-8'
7.2 目录操作
os模块：
os.path.exists() - 检查路径是否存在
os.path.join() - 路径拼接
os.path.dirname() - 获取目录名
os.path.abspath() - 获取绝对路径
os.makedirs() - 创建目录
os.remove() - 删除文件
os.path.getsize() - 获取文件大小
os.path.isfile() - 判断是否为文件
7.3 文件路径
路径操作：相对路径、绝对路径
路径拼接：os.path.join(MEMORY_FOLDER, memory_file)
路径检查：os.path.exists(path)
八、JSON处理
8.1 JSON序列化
json.dump()：写入文件
  with open(MEMORY_FILE, 'w', encoding='utf-8') as f:      json.dump(data, f, ensure_ascii=False, indent=2)
json.dumps()：转换为字符串
  string json = json.dumps(requestData)
8.2 JSON反序列化
json.load()：从文件读取
  with open(MEMORY_FILE, 'r', encoding='utf-8') as f:      data = json.load(f)
json.loads()：从字符串解析
  message = json.loads(message)
8.3 JSON操作
嵌套访问：result['choices'][0]['message']['content']
安全访问：data.get('history', [])
类型检查：isinstance(data, dict), isinstance(data, list)
九、HTTP请求
9.1 requests库
GET请求：
  response = requests.get(url, headers=headers)
POST请求：
  response = requests.post(url, headers=headers, json=data, timeout=30)
PUT请求：
  response = requests.put(url, json=data, headers=headers)
9.2 请求参数
headers：请求头设置
  headers = {      "Authorization": f"Bearer {ZHIPU_API_KEY}",      "Content-Type": "application/json"  }
json参数：自动序列化JSON
timeout参数：超时设置
response.encoding：响应编码设置
9.3 响应处理
状态码检查：response.status_code == 200
响应内容：response.text, response.json(), response.content
错误处理：根据状态码判断请求是否成功
十、WebSocket通信
10.1 WebSocket连接
websocket库：import websocket
WebSocketApp：创建WebSocket应用
  ws = websocket.WebSocketApp(wsUrl,                              on_message=on_message,                              on_error=on_error,                              on_close=on_close)
10.2 WebSocket事件
on_message：接收消息回调
on_error：错误处理回调
on_close：关闭连接回调
on_open：连接打开回调
10.3 WebSocket操作
发送消息：ws.send(json.dumps(data))
关闭连接：ws.close()
运行循环：ws.run_forever(sslopt={"cert_reqs": ssl.CERT_NONE})
十一、加密与编码
11.1 哈希算法
hashlib模块：
  signature_sha = hmac.new(api_secret.encode('utf-8'),                           signature_origin.encode('utf-8'),                          digestmod=hashlib.sha256).digest()
11.2 Base64编码
base64模块：
  base64.b64encode(data).decode(encoding='utf-8')  base64.b64decode(encoded_data)
11.3 HMAC认证
hmac模块：用于API签名认证
十二、时间处理
12.1 datetime模块
datetime对象：
  from datetime import datetime  datetime.now().strftime("%Y-%m-%d %H:%M:%S")  datetime.now().isoformat()
时间格式化：strftime(), isoformat()
12.2 time模块
time.time()：获取时间戳
time.sleep()：延时
time.mktime()：时间元组转时间戳
十三、第三方库应用
13.1 pygame库
初始化：pygame.init()
窗口创建：pygame.display.set_mode((WIDTH, HEIGHT))
事件处理：pygame.event.get()
图形绘制：pygame.draw.rect(), pygame.draw.circle()
字体渲染：pygame.font.Font(), font.render()
图像处理：pygame.Surface(), pygame.transform.rotate()
音频播放：pygame.mixer.init(), pygame.mixer.music.load(), pygame.mixer.music.play()
时钟控制：pygame.time.Clock(), clock.tick(60)
13.2 streamlit库
页面配置：st.set_page_config()
状态管理：st.session_state
UI组件：
st.title(), st.header(), st.subheader()
st.selectbox() - 下拉选择框
st.button() - 按钮
st.chat_input() - 聊天输入框
st.chat_message() - 聊天消息
st.spinner() - 加载动画
st.info(), st.error() - 信息提示
布局：st.sidebar, with st.sidebar:
查询参数：st.query_params.get()
重新运行：st.rerun(), st.stop()
13.3 requests库
HTTP方法：GET, POST, PUT
请求参数：headers, json, timeout
响应处理：status_code, text, json()
十四、高级特性
14.1 字符串格式化
f-string：f"速度：{speed} km/h"
format()方法："host: {}\ndate: {}".format(host, date)
%格式化："%s" % value
14.2 列表推导式
基本用法：[item.get('content', '') for item in data if isinstance(item, dict)]
条件过滤：带if条件的列表推导式
14.3 字典推导式
基本用法：{key: value for key, value in items}
14.4 生成器表达式
生成器：(x for x in range(10))
14.5 上下文管理器
with语句：
  with open(file, 'r') as f:      data = f.read()
14.6 装饰器
函数装饰器：@staticmethod, @classmethod
自定义装饰器：创建装饰器函数
14.7 多线程
thread模块：thread.start_new_thread(run, ())
线程同步：全局变量控制线程状态
14.8 全局变量
global关键字：
  global tts_audio_file, tts_complete
十五、代码组织
15.1 函数式编程
函数作为参数：回调函数
高阶函数：接受函数作为参数的函数
15.2 模块化设计
功能分离：不同功能放在不同模块
导入导出：模块间的依赖关系
命名空间：避免命名冲突
15.3 代码复用
函数封装：将重复代码封装成函数
类封装：将相关功能封装成类
模块复用：在不同项目中复用模块
十六、最佳实践
16.1 错误处理
异常捕获：使用try-except处理异常
错误日志：记录错误信息
优雅降级：异常时的备用方案
16.2 代码风格
命名规范：变量、函数、类的命名
注释规范：代码注释和文档字符串
代码格式：PEP 8规范
16.3 性能优化
列表vs生成器：根据需求选择
缓存机制：避免重复计算
异步处理：多线程、协程
十七、项目特定技术
17.1 API集成
RESTful API：HTTP请求调用API
WebSocket API：实时通信
API认证：Bearer Token, API Key
错误处理：API调用失败处理
17.2 数据持久化
JSON文件：存储对话历史
文件管理：创建、读取、更新、删除文件
17.3 状态管理
游戏状态：状态机模式
会话状态：streamlit的session_state
全局状态：全局变量管理
17.4 用户交互
命令行交互：input()函数
图形界面：pygame窗口交互
Web界面：streamlit Web应用
事件驱动：事件处理机制