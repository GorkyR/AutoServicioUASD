using System;
using System.Collections;
using System.Collections.Generic;

namespace UASD
{
    public class CourseCollection : ICollection<Course>
    {
        public string Name { get; set; }

        public CourseCollection() { }
        public CourseCollection(string name) { this.Name = name; }

        public delegate void CourseCollectionChangedHandler(CourseCollection collection);

        public event CourseCollectionChangedHandler CollectionChanged;

        public bool IsValidSchedule
        {
            get
            {
                foreach (Course a in this.Courses)
                    foreach (Course b in this.Courses)
                        if (a != b)
                            if (a.CollidesWith(b))
                                return false;
                return true;
            }
        }
        public bool IsCompatibleWith(Course item)
        {
            foreach (Course m in this.Courses)
                if (m.CollidesWith(item))
                    return false;
            return true;
        }

        public override string ToString() => $"{Name} - [{Count}]";

        #region ICollection Interface implementation
            private List<Course> Courses = new List<Course>();
            public Course this[int index] {
                get => this.Courses[index];
                set {
                    this.Courses[index] = value;
                    this.CollectionChanged?.Invoke(this);
                }
            }
            public int Count => this.Courses.Count;
            public bool IsReadOnly => ((ICollection<Course>)Courses).IsReadOnly;
            public void Add(Course item) {
                this.Courses.Add(item);
                this.CollectionChanged?.Invoke(this);
            }
            public void Remove(Course item) {
                this.Courses.Remove(item);
                this.CollectionChanged?.Invoke(this);
            }
            public void Remove(int index) {
                this.Courses.RemoveAt(index);
                this.CollectionChanged?.Invoke(this);
            }
            public void Clear() {
                Courses.Clear();
                this.CollectionChanged?.Invoke(this);
            }
            public bool Contains(Course item) { return Courses.Contains(item); }
            public void CopyTo(Course[] array, int arrayIndex) { Courses.CopyTo(array, arrayIndex); }
            bool ICollection<Course>.Remove(Course item) { return Courses.Remove(item); }
            public IEnumerator<Course> GetEnumerator() { return Courses.GetEnumerator(); }
            IEnumerator IEnumerable.GetEnumerator() { return Courses.GetEnumerator(); }
        #endregion
    }
}
