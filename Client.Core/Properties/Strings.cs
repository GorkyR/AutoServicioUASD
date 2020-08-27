namespace UASD.Properties
{
    public struct Strings
    {
        public const string BaseUrl                     = "http://www.autoservicio.uasd.edu.do/";
        public const string LoginUrl                    = "http://www.autoservicio.uasd.edu.do/PROD/twbkwbis.P_ValLogin";
        public const string HorarioDetalleURL           = "http://www.autoservicio.uasd.edu.do/PROD/bwskfshd.P_CrseSchdDetl";
        public const string ProyeccionUrl               = "http://www.autoservicio.uasd.edu.do/PROD/bvsfkpro.P_Projection_Page";
        public const string CalificacionesSelectionUrl  = "http://www.autoservicio.uasd.edu.do/PROD/bwskmgrd.p_write_term_selection";
        public const string CalificacionesUrl           = "http://www.autoservicio.uasd.edu.do/PROD/bwskmgrd.p_write_midterm_grades";
        public const string BuscarClasesSelectionUrl    = "http://www.autoservicio.uasd.edu.do/PROD/bwskfcls.p_sel_crse_search";
        public const string BuscarClasesSelection2Url   = "http://www.autoservicio.uasd.edu.do/PROD/bwckgens.p_proc_term_date";
        public const string BuscarClasesUrl             = "http://www.autoservicio.uasd.edu.do/PROD/bwskfcls.P_GetCrse";
        public const string EvaluacionSelectionUrl      = "http://www.autoservicio.uasd.edu.do/PROD/bwckcapp.P_DispCurrent";
        public const string KardexUrl                   = "http://www.autoservicio.uasd.edu.do/PROD/bwskotrn.P_ViewTran"; // ?levl=&tprt=INTL
        public const string RegistroSeleccionUrl        = "http://www.autoservicio.uasd.edu.do/PROD/bwskfreg.P_AltPin";
        public const string RegistroUrl                 = "http://www.autoservicio.uasd.edu.do/PROD/bwckcoms.P_Regs";
        public const string CalendarioSeleccionUrl      = "http://www.autoservicio.uasd.edu.do/PROD/bwskrsta.P_RegsStatusDisp";

        public const string BuscarClasesDummyParameters = "&sel_ptrm=%25&begin_hh=0&begin_mi=0&end_hh=0&end_mi=0&begin_ap=x&end_ap=y&path=1&SUB_BTN=Buscar+Curso&SEL_CRSE=&SEL_TITLE=&SEL_DAY=&SEL_SCHD=&SEL_SESS=&SEL_INSTR=&SEL_ATTR=&SEL_LEVL=&SEL_INSM=&CRN=&RSTS=0";
        public const string ClasesDummyParameters       = "&path=1&SUB_BTN=Ver+Secciones+Existentes+&BEGIN_HH=0&BEGIN_MI=0&BEGIN_AP=a&END_HH=0&END_MI=0&END_AP=a&SEL_INSTR=%25&SEL_ATTR=%25&SEL_LEVL=%25&SEL_TITLE=&SEL_DAY=&SEL_PTRM=&SEL_CAMP=&SEL_SCHD=&SEL_SESS=&SEL_INSM=&CRN=&RSTS=&";

        public const string NotLoggedInMessage          = "Usted ha sido desconectado del AutoServicio.\nDebe ingresar nuevamente.\n\nEsto sucede cuando ingresa al AutoServicio a través de la pagina principal u otra instancia del cliente, o cuando no interactua por más de media hora.";
        public const string NoProyectionMessage         = "Su proyeccion no esta disponible en este momento.";
        public const string NoSelectionMessage          = "No es posible seleccionar en estos momentos.";
        public const string ExpiredLoginMessage         = "Su NIP ha expirado. Proceda al portal web del autoservicio para cambiarlo.";
    }
}
