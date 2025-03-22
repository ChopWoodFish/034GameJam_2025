using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class UpdateComp : IDisposable
    {
        // 正在tick的方法
        private Dictionary<Action, List<ScheduleData>> dictEventSchedule = new Dictionary<Action, List<ScheduleData>>();

        // 即将执行的方法
        private List<Action> listToExecuteAction = new List<Action>();
        
        // 即将添加的方法
        private Dictionary<Action, List<ScheduleData>> dictToAddEventSchedule = new Dictionary<Action, List<ScheduleData>>();

        // 即将移除的方法
        private List<ScheduleData> listToRemoveSchedule = new List<ScheduleData>();
        private List<Action> listToRemoveAction = new List<Action>();

        public UpdateComp()
        {
            if (MonoUpdater.OnUpdate == null)
            {
                MonoUpdater.OnUpdate = Update;
            }
            else
            {
                MonoUpdater.OnUpdate += Update;
            }
        }

        public void ScheduleAction(Action action, float waitTime, int actTime)
        {
            ScheduleData schedule = new ScheduleData(waitTime, actTime);
            if (!dictToAddEventSchedule.ContainsKey(action))
            {
                dictToAddEventSchedule.Add(action, new List<ScheduleData>());
            }
            dictToAddEventSchedule[action].Add(schedule);
        }

        public void DelayAction(Action action, float delayTime)
        {
            ScheduleAction(action, delayTime, 1);
        }

        // 注意似乎有可能还是会tick一帧后才被移除
        public void StopSchedule(Action action)
        {
            if (dictEventSchedule.ContainsKey(action))
            {
                listToRemoveAction.Add(action);
            }
            else if (dictToAddEventSchedule.ContainsKey(action))
            {
                dictToAddEventSchedule.Remove(action);
            }
        }

        private void Update()
        {
            UpdateSchedule();
            ExecuteAction();
            CleanDisusedAction();
            AddNewSchedule();
        }

        private void UpdateSchedule()
        {
            foreach (var eventSchedulePair in dictEventSchedule)
            {
                var action = eventSchedulePair.Key;
                var listSchedule = eventSchedulePair.Value;
                
                // tick
                foreach (var schedule in listSchedule)
                {
                    bool isTimerUp = schedule.Elapse();
                    if (isTimerUp)
                    {
                        listToExecuteAction.Add(action);
                        bool hasLeftTime = schedule.SubOneTime();
                        if (!hasLeftTime)
                        {
                            listToRemoveSchedule.Add(schedule);
                        }
                    }
                }

                // remove
                foreach (var toRemoveSchedule in listToRemoveSchedule)
                {
                    listSchedule.Remove(toRemoveSchedule);
                }
                if (listSchedule.Count == 0)
                {
                    listToRemoveAction.Add(action);
                }
                listToRemoveSchedule.Clear();
            }
        }

        private void ExecuteAction()
        {
            foreach (var action in listToExecuteAction)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    // Console.WriteLine(e);
                    // throw;
                }
            }
            listToExecuteAction.Clear();
        }

        private void CleanDisusedAction()
        {
            foreach (var toRemoveAction in listToRemoveAction)
            {
                dictEventSchedule.Remove(toRemoveAction);
            }
            listToRemoveAction.Clear();
        }

        private void AddNewSchedule()
        {
            foreach (var eventSchedulePair in dictToAddEventSchedule)
            {
                var newAction = eventSchedulePair.Key;
                var newListSchedule = eventSchedulePair.Value;
                if (!dictEventSchedule.ContainsKey(newAction))
                {
                    dictEventSchedule.Add(newAction, new List<ScheduleData>());
                }
                dictEventSchedule[newAction].AddRange(newListSchedule);
            }
            dictToAddEventSchedule.Clear();
        }

        public void Dispose()
        {
            if (MonoUpdater.OnUpdate != null)
            {
                MonoUpdater.OnUpdate -= Update;
            }
            dictEventSchedule.Clear();
        }
    }
}