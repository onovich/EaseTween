# EaseTween  
EaseTween, a timing management library to replace DOTween.  <br/>
**EaseTween，用于替代 DOTween 的时序动画管理库，取名自"缓动"。**  

EaseTween provides Burst-compiled zero-GC tweening with pure data processing, supporting positions/colors/rotations and chainable calls.  <br/>
**EaseTween 提供基于 Burst 编译的零 GC 补间，纯数据处理，支持位移/颜色/旋转，支持链式调用。**  

The library has no GameObject/MonoBehaviour dependency, ideal for data-driven architectures.  <br/>
**不依赖任何游戏对象，无 IoC，适合数据驱动架构。**

# Readiness  
Stable and production-ready.  <br/>
**稳定可用于生产环境。**  

# Sample  
```
// 初始化
TweenCore tween = new TweenCore(); 

// 创建补间
int colorTweenId = tween.Create(
    start: Vector.red, 
    end: Color.blue, 
    duration: 2f, 
    easing: EasingType.SineInOut
);

int moveTweenId = tween.Create(
    start: Vector3.zero,
    end: Vector3.forward * 5,
    duration: 1f
);

// 链接
tween.Link(tweenId, moveId);

// 绑定回调
tween.OnUpdate(colorTweenId, (color) => SetColor(color)); 
tween.OnComplete(tweenId, (pos) => Move(pos));
```
# UPM URL  
**Main**  
`ssh://git@github.com/onovich/EaseTween.git?path=/Assets/com.mortise.easetween#main`  