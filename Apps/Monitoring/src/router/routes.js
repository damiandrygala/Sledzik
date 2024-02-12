import DevsPage from "pages/DevsPage.vue";
import ZonesPage from "pages/ZonesPage.vue";
import AnalysisPage from "pages/AnalysisPage.vue";

const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      { path: "", component: () => import("pages/MainPage.vue") },
      {
        path: "devs",
        component: DevsPage,
      },
      {
        path: "zones",
        component: ZonesPage,
      },
      {
        path: "analysis",
        component: AnalysisPage,
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
