using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 无功功率补偿
{
    class GreedyCapacitorAllocator
    {
        /// <summary>
        /// 计算达成目标补偿值所需的电容组合
        /// </summary>
        /// <param name="targetCompensation">目标补偿值（VAR）</param>
        /// <param name="capacitorTypes">可用电容类型（容量和最大数量）</param>
        /// <returns>电容分配结果</returns>
        public Dictionary<int, int> Allocate(int targetCompensation, Dictionary<int, int> capacitorTypes)
        {
            // 验证输入参数
            if (targetCompensation <= 0)
                throw new ArgumentException("目标补偿值必须大于0", nameof(targetCompensation));

            if (capacitorTypes == null || !capacitorTypes.Any())
                throw new ArgumentException("必须提供至少一种电容类型", nameof(capacitorTypes));

            // 按电容容量降序排序（贪心算法核心：优先使用大容量）
            var sortedCapacitors = capacitorTypes
                .OrderByDescending(kv => kv.Key)
                .ToList();

            int remaining = targetCompensation;
            var allocation = new Dictionary<int, int>();

            // 初始化分配结果
            foreach (var cap in sortedCapacitors)
            {
                allocation[cap.Key] = 0;
            }

            // 贪心分配过程
            foreach (var (capacity, maxCount) in sortedCapacitors)
            {
                // 已完成目标补偿，退出循环
                if (remaining <= 0)
                    break;

                // 计算当前电容类型最多可使用的数量
                // 不能超过最大可用数量，也不能超过剩余补偿需求
                int possibleCount = Math.Min(remaining / capacity, maxCount);

                // 如果有剩余容量但无法整除，需要多使用一个
                if (possibleCount * capacity < remaining && possibleCount < maxCount)
                {
                    possibleCount++;
                }

                // 记录使用数量并更新剩余补偿值
                allocation[capacity] = possibleCount;
                remaining -= possibleCount * capacity;
            }

            // 检查是否完全补偿
            if (remaining > 0)
            {
                throw new InvalidOperationException(
                    $"无法完全补偿目标值 {targetCompensation}VAR，" +
                    $"剩余 {remaining}VAR 无法满足，可能需要增加电容数量或类型");
            }
            return allocation;
        }       
    }
}
