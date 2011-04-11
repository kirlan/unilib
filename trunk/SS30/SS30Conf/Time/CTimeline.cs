using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS30Conf.Actions;
using SS30Conf.Persons;

namespace SS30Conf.Time
{
    class CTimeline
    {
        private List<CTimelineRecord> m_cRecords = new List<CTimelineRecord>();

        /// <summary>
        /// Добавляет в очередь событий событие окончания указанного действия.
        /// Считаем, что действие выполняется в момент, когда оно добавляется, т.е.
        /// событию окончания действия присваивается временная метка равная длительности
        /// действия.
        /// </summary>
        /// <param name="pAction">Действие, окончание которого добавляем в очередь</param>
        /// <param name="pActor">Исполнитель</param>
        /// <param name="pTarget">Цель</param>
        public void AddRecord(CTimelineRecord pNewRecord)
        {
            m_cRecords.Add(pNewRecord);
            GetLeadRecord();
        }

        private CTimelineRecord m_pLeadRecord = null;

        /// <summary>
        /// Очищает очередь событий от окончаний действий.
        /// Вызывается при завершении времени суток
        /// </summary>
        public void ClearActions()
        {
            List<CTimelineRecord> cErased = new List<CTimelineRecord>();
            foreach (CTimelineRecord record in m_cRecords)
            {
                if (record.Type == RecordType.ACTION_END)
                    cErased.Add(record);
            }
            foreach (CTimelineRecord record in cErased)
            {
                m_cRecords.Remove(record);
            }
            GetLeadRecord();
        }

        /// <summary>
        /// Вершина очереди событий - событие с нулевой временной меткой,
        /// т.е. событие, время которого пришло.
        /// Если в вершине очереди реально находится событие, время которого ещё не пришло
        /// (т.е. временная метка больше нуля), то возвращается null. Для того, чтобы получить
        /// доступ к этому событию, необходимо дождаться его, выполнив функцию PassTime().
        /// Если очередь событий пуста, то так же возвращается null.
        /// </summary>
        public CTimelineRecord LeadRecord
        {
            get 
            {
                if (m_pLeadRecord.TimeLabel == 0)
                    return m_pLeadRecord;
                else
                    return null;
            }
        }

        /// <summary>
        /// Помещает в переменную m_pLeadRecord запись из очереди с минимальной временной меткой.
        /// </summary>
        private void GetLeadRecord()
        { 
            if(m_cRecords.Count == 0)
                return;
            
            if(m_pLeadRecord == null)
                m_pLeadRecord = m_cRecords[0];

            foreach (CTimelineRecord pRecord in m_cRecords)
            {
                if (pRecord.TimeLabel < m_pLeadRecord.TimeLabel)
                    m_pLeadRecord = pRecord;
            }
        }

        /// <summary>
        /// Пропускает время до следующего по очерёдности события в очереди.
        /// Если в вершине очереди событий находится событие с нулевой временной меткой,
        /// то УДАЛЯЕТ его и помещает в вершину событие с минимальной временной меткой 
        /// из числа оставшихся (быть может - тоже с нулевой).
        /// После чего смещает временные метки всех событий в очереди таким образом,
        /// чтобы метка помещённого в вершину события стала нулевой.
        /// Возвращает значение, на которое были смещены метки всех событий в очереди,
        /// т.е. время, прошедшее до следующего события.
        /// 
        /// После вызова этой функции, перед её следующим вызовом, обязательно нужно считать
        /// LeadRecord, иначе информация о событии, случившемся после ожидания, будет потеряна.
        /// </summary>
        /// <returns>прошедшее время</returns>
        public int PassTime()
        { 
            if(m_pLeadRecord == null)
                return 0;

            int iTimePassed = m_pLeadRecord.TimeLabel;

            if (iTimePassed == 0)
            {
                m_cRecords.Remove(m_pLeadRecord);
                m_pLeadRecord = null;
                GetLeadRecord();
                iTimePassed = m_pLeadRecord.TimeLabel;
            }

            foreach (CTimelineRecord pRecord in m_cRecords)
            {
                pRecord.PassTime(iTimePassed);
            }

            return iTimePassed;
        }
    }
}
