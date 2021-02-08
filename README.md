# UnityWebMediapipePose
将 Mediapipe Pose识别与Unity WebGL接入

# 说明
Unity web jS与unity 通信文档

思路
从PlaybackEngines/WebGLSupport/BuildTools/lib 里webcam 脚本可知道 unity webgl 播放视频就是使用一个<video></video>标签
而Mediapipe 的js版本也是把camera图像给到video显示并传给pose计算骨骼
所以可以使用unity 打开摄像头并显示图像 mediapipe只需要取得unity创建的video标签并计算返回数据就可

我们把 mediapipe计算解析的代码写在 https://github.com/jsaon-L/UnityWebMediapipePose/blob/main/wwwroot/index.html 这里(这个index.html 是unity打包生成)
之后使用上面文档里的通讯方法 进行通讯就可

按照文档 Unity和js通信需要编写 .jslib 插件脚本(不知道Unity能否直接调用index.html里的js函数),所以我们写一个函数作为中间函数


Unity   js.jslib    HTML(pose脚本)

大体流程为 `Unity`发送请求计算骨骼给-->`.jslib`-->`HTML(计算骨骼)`-->`Unity(将结果通知Unity)`
