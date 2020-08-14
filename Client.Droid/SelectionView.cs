using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Annotation;
using Android.Support.Constraints;
using Android.Support.Design.Resources;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using UASD;

namespace Client.Droid
{
    public class SelectionView : FrameLayout
    {
        public LinearLayout SelectedLinearLayout;
        public LinearLayout AvailableLinearLayout;

        private List<OpenCourse>       SelectedCourses = new List<OpenCourse>();
        private List<CourseCollection> AvailableCourses;

        private Action OnSelection;

        public SelectionView(Context context): base(context)
        {
            Inflate(context, Resource.Layout.view_selection, this);

            var textUnavailable = FindViewById<TextView>(Resource.Id.text_unavailable);
            textUnavailable.Visibility = ViewStates.Visible;
        }

        public SelectionView(Context context, List<CourseCollection> availableCourses, Action onSelection) : base(context)
        {
            Inflate(context, Resource.Layout.view_selection, this);

            SelectedLinearLayout  = FindViewById<LinearLayout>(Resource.Id.layout_selected_list);
            AvailableLinearLayout = FindViewById<LinearLayout>(Resource.Id.layout_available_list);

            AvailableCourses = availableCourses;

            OnSelection = onSelection;

            UpdateLists();
        }

        private void UpdateLists()
        {
            SelectedLinearLayout.RemoveAllViews();
            AvailableLinearLayout.RemoveAllViews();
            SelectedLinearLayout.SetPadding(0, 0, 0, 0);

            if (SelectedCourses.Count() > 0)
            { // Selected
                var titleText = new TextView(Context) { Text = "Selección" };
                titleText.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Large);
                titleText.SetTextColor(Resources.GetColor(Resource.Color.material_grey_800));
                titleText.Typeface = Typeface.DefaultBold;
                titleText.SetPadding(0, 0, 0, 16);

                SelectedLinearLayout.AddView(titleText);

                var card = new CardView(Context) { CardElevation = 6 };
                card.SetContentPadding(16, 16, 16, 16);
                var cardLayoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                cardLayoutParams.SetMargins(16, 16, 16, 16);
                card.LayoutParameters = cardLayoutParams;

                var cardLayout = new LinearLayout(Context) { Orientation = Orientation.Vertical };
                card.AddView(cardLayout);

                foreach (var course in SelectedCourses)
                {
                    var itemSelected = new SelectedCourseItemView(Context, course, () =>
                    {
                        SelectedCourses.Remove(course);
                        UpdateLists();
                    });
                    itemSelected.SetPadding(0, 0, 0, 24);
                    cardLayout.AddView(itemSelected);
                }

                var buttonSelect = new Button(Context)
                {
                    Background = Resources.GetDrawable(Resource.Color.colorAccent),
                    Text       = "Inscribir selección",
                    Elevation  = 8
                };
                buttonSelect.SetPadding(0, 16, 0, 16);
                var buttonLayoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                buttonLayoutParams.SetMargins(32, 32, 32, 16);
                buttonSelect.LayoutParameters = buttonLayoutParams;

                buttonSelect.Click += async (s, e) => {
                    var selection = (from course in SelectedCourses
                                     select course.NRC).ToArray();

                    void makeFinalDialog(string title, string message)
                    {
                        LayoutParams wrapContent() => new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

                        var dialog = new Dialog(Context);
                        dialog.Window.AddFlags(WindowManagerFlags.NotTouchModal);

                        var dialogLayout = new LinearLayout(Context) {
                            Orientation = Orientation.Vertical,
                            LayoutParameters = wrapContent(),
                        };
                        dialogLayout.SetPadding(32, 32, 32, 32);

                        var textTitle = new TextView(Context) {
                            Text = title,
                            LayoutParameters = wrapContent(),
                        };
                        textTitle.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Medium);
                        textTitle.SetTextColor(Color.Black);
                        textTitle.SetPadding(0, 0, 0, 16);
                        dialogLayout.AddView(textTitle);

                        var textMessage = new TextView(Context) { Text = message };
                        textMessage.SetPadding(0, 0, 0, 16);
                        textMessage.LayoutParameters = wrapContent();
                        dialogLayout.AddView(textMessage);

                        var buttonDismiss = new Button(Context, null, Resource.Style.Base_Widget_AppCompat_Button_Borderless_Colored) {
                            Text = "Entendido",
                            Typeface = Typeface.DefaultBold,
                            TextSize = 16,
                            Gravity = GravityFlags.Right,
                        };
                        buttonDismiss.SetTextColor(Resources.GetColor(Resource.Color.colorAccent));
                        buttonDismiss.Click += (s, e) => {
                            dialog.Dismiss();
                        };

                        dialogLayout.AddView(buttonDismiss);

                        dialog.SetContentView(dialogLayout);
                        dialog.Show();
                    }

                    switch(ClientStateService.dataSource)
                    {
                        case ClientStateService.DataSource.Production: {
                            try
                            {
                                await ClientStateService.AutoServicio.RegisterCoursesAsync(selection);
                                makeFinalDialog("Exito", "Inscripción completada con éxito.");
                            } catch (NoSelectionAvailableException) {
                                makeFinalDialog("No disponible", "La inscripción de materias no está disponible en estos momentos.");
                            } catch (SeleccionErrorsException see) {
                                makeFinalDialog("Error", $"La inscripción reportó error para las siguientes materias:\n{see.Message}");
                            }
                        }
                        break;
                        case ClientStateService.DataSource.Random:{
                            makeFinalDialog("Inscripción de prueba completada", $"Lista de materias seleccionadas:\n[{string.Join(", ", selection)}]");
                        } break;
                        default:
                            throw new IllegalStateException();
                    }


                    SelectedCourses.Clear();
                    OnSelection();
                };

                cardLayout.AddView(buttonSelect);
                SelectedLinearLayout.AddView(card);

                SelectedLinearLayout.SetPadding(0, 0, 0, 32);
            }

            { // Available
                var titleText = new TextView(Context) { Text = "Disponibles" };
                titleText.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Large);
                titleText.SetTextColor(Resources.GetColor(Resource.Color.material_grey_800));
                titleText.Typeface = Typeface.DefaultBold;
                titleText.SetPadding(0, 0, 0, 16);

                AvailableLinearLayout.AddView(titleText);

                foreach (var collection in AvailableCourses)
                {
                    IEnumerable<OpenCourse> filteredCollection;
                    if (SelectedCourses.Any(sc => sc.Title == collection.Name))
                        filteredCollection = new List<OpenCourse>();
                    else
                        filteredCollection = from course in collection
                                             where !SelectedCourses.Any(sc => sc.CollidesWith(course))
                                             select course as OpenCourse;

                    var card = new CardView(Context) { CardElevation = 6 };
                    card.SetContentPadding(16, 16, 16, 16);
                    var cardLayoutParams = new LinearLayout.LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
                    cardLayoutParams.SetMargins(16, 16, 16, 16);
                    card.LayoutParameters = cardLayoutParams;

                    var cardLayout = new LinearLayout(Context) { Orientation = Orientation.Vertical };

                    var textTitle = new TextView(Context) { Text = $"{collection.Name} ({filteredCollection.Count()}/{collection.Count})", };
                    textTitle.SetTextAppearance(Resource.Style.TextAppearance_AppCompat_Medium);
                    textTitle.SetTextColor(Color.Black);
                    textTitle.Typeface = Typeface.DefaultBold;
                    textTitle.SetPadding(0, 0, 0, 16);

                    cardLayout.AddView(textTitle);
                    //AvailableLinearLayout.AddView(textTitle);

                    foreach (var course in filteredCollection)
                    {
                        var itemAvailable = new AvailableCourseItemView(Context, course, () =>
                        {
                            SelectedCourses.Add(course);
                            UpdateLists();
                        });
                        itemAvailable.SetPadding(0, 0, 0, 24);
                        cardLayout.AddView(itemAvailable);
                        //AvailableLinearLayout.AddView(itemAvailable);
                    }

                    card.AddView(cardLayout);
                    AvailableLinearLayout.AddView(card);
                }
            }
        }
    }
}