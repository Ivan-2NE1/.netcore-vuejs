import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";

import { ToastPlugin } from 'bootstrap-vue';
Vue.use(ToastPlugin);

var gameDashboard = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper
    },

    data: {

    }
});
