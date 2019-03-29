using Syncfusion.SfCalendar.XForms;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FilteringEvents
{
    public class CalendarBehavior: Behavior<ContentPage>
    {
        private SfCalendar calendar;
        private SfComboBox comboBox;
        private CalendarEventCollection calendarInlineEvents = new CalendarEventCollection();
        public CalendarBehavior()
        {
        }

        protected override void OnAttachedTo(ContentPage bindable)
        {
            var page = bindable as Page;
            calendar = page.FindByName<SfCalendar>("calendar");
            comboBox = page.FindByName<SfComboBox>("comboBox");
            comboBox.Watermark = "Search Events";

            // To Display Inline appointment layout.
            calendar.ShowInlineEvents = true;
            calendar.InlineViewMode = InlineViewMode.Agenda;
            var currentDay = DateTime.Now.Date;

            for (int i = 0; i < 40; i++)
            {
                calendarInlineEvents.Add(new CalendarInlineEvent()
                {
                    StartTime = currentDay.AddDays(i),
                    EndTime = currentDay.AddDays(i),
                    Subject = "Meeting on " + currentDay.AddDays(i).ToString("dd/MM/yy"),
                    Color = Color.Fuchsia
                });

                calendarInlineEvents.Add(new CalendarInlineEvent()
                {
                    StartTime = currentDay.AddDays(i),
                    EndTime = currentDay.AddDays(i),
                    Subject = "Conference on " + currentDay.AddDays(i).ToString("dd/MM/yy"),
                    Color = Color.DodgerBlue
                });
            }

            // Setting Datasource for calendar.
            calendar.DataSource = calendarInlineEvents;

            // Setting Data Source for combo box
            comboBox.DataSource = calendarInlineEvents;
            // Displaying Data in combo box based on subject
            comboBox.DisplayMemberPath = "Subject";
            comboBox.SelectionChanged += ComboBox_SelectionChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            comboBox.SelectionChanged -= ComboBox_SelectionChanged;
            base.OnDetachingFrom(bindable);
        }

        /// <summary>
        /// Gets the appointments based on subject.
        /// </summary>
        /// <returns>The appointments.</returns>
        /// <param name="subject">Subject.</param>
        private CalendarEventCollection GetAppointments(string subject)
        {
            var data = new CalendarEventCollection();
            foreach (var item in calendarInlineEvents)
            {
                if (item.Subject == subject)
                    data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// Combos the box selection changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void ComboBox_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            // Returning when value cleared in combo box
            if ((e.Value as CalendarInlineEvent) == null || String.IsNullOrEmpty((e.Value as CalendarInlineEvent).Subject))
                return;

            // Setting Calendar DataSource based on subject filtered data.
            calendar.DataSource = GetAppointments((e.Value as CalendarInlineEvent).Subject);
            calendar.MoveToDate = GetAppointments((e.Value as CalendarInlineEvent).Subject)[0].StartTime;
        }
    }
}
