using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Mortise.EaseTween {

    public static class TweenJobBurstInitializer {

        [RuntimeInitializeOnLoadMethod]
        private static void SafeForceBurstCompilation() {
            // 1. 准备初始数据
            var initialData = new TweenModel {
                id = 1,
                type = TweenType.Float,
                duration = 1.0f,
                isPlaying = true
            };

            // 2. 创建并填充NativeArray
            using (var tempArray = new NativeArray<TweenModel>(
                new[] { initialData },  // 通过构造函数初始化
                Allocator.TempJob)) {
                // 3. 执行Job
                var job = new TweenUpdateJob {
                    tweens = tempArray,
                    deltaTime = 0.016f
                };
                job.ScheduleParallel(1, 1, default).Complete();
            }
        }
    }

}