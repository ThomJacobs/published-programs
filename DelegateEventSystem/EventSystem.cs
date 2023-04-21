using System.Collections.Generic;

namespace Jacobs.Events
{
    public class EventSystem : Core.MonoSingleton<EventSystem>
    {
        //Attributes:
        public delegate void EventFunctionReference(IEventData p_data);
        private Dictionary<System.Type, List<EventFunctionReference>> m_subscribers = new Dictionary<System.Type, List<EventFunctionReference>>();

        //Constructors:
        public EventSystem() { m_subscribers = new Dictionary<System.Type, List<EventFunctionReference>>(); }

        //Methods:
        [UnityEditor.MenuItem("Extended/Event System Instance")]
        private static void SelectInstance()
        {
            //The singleton property will do the work for creating and initialising the event system instance.
            UnityEditor.Selection.activeObject = Singleton.gameObject;
        }

        /**
         *  Adds a reference to a function/method which will be invoked when a trigger of the same 'event-data-type' occurs.
         *  
         *  @param p_subscriber: The function/method intended to be added to the event system.
         */
        public void Subscribe<EventDataType>(EventFunctionReference p_subscriber) where EventDataType : IEventData
        {
            //Cache the system type value.
            System.Type systemType = typeof(EventDataType);

            //If list for the specified event type has not been initialised, define and initalise a new list.
            if (!m_subscribers.ContainsKey(systemType)) { m_subscribers.Add(systemType, new List<EventFunctionReference>()); }

            m_subscribers[systemType].Add(p_subscriber); //Store delegate under it's specified 'event-type'.
        }

        /**
         *  Invokes all functions/methods subscribed to the event system that parameterise the 'EventDataType'.
         *  
         *  @param p_eventData (IEventData): Data/information that will passed over to all functions/methods subscribed to its type.
         */
        public void Trigger<EventDataType>(EventDataType p_eventData) where EventDataType : IEventData
        {
            //Cache the system type value.
            System.Type systemType = typeof(EventDataType);

            if (!m_subscribers.ContainsKey(systemType)) { return; } //If no delegates are subscribed to specified the event-type, exit method.

            foreach (EventFunctionReference subscriber in m_subscribers[systemType])
            {
                subscriber.Invoke(p_eventData); //Invoke each method subscribed to specified event type.
            }
        }

        /**
         *  Removes a method/function reference from the event system.
         *  
         *  @param p_subscriber (EventFunctionReference): A method/function reference currently subscribed to the system.
         */
        public void Unsubscribe<EventDataType>(EventFunctionReference p_subscriber) where EventDataType : IEventData
        {
            //Cache the system type value.
            System.Type systemType = typeof(EventDataType);

            if (!m_subscribers.ContainsKey(systemType)) return;
            m_subscribers[systemType].Remove(p_subscriber);
        }
    }
}