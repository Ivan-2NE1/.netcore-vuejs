import VueSelectBase from "../../components/base/vue-selectbase/vue-selectbase.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var sportPositions = new Vue({
    el: '#vueApp',
    components: {
        "vue-selectbase": VueSelectBase,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar
    },

    data: {
        sportId: window.sportId,
        position: window.position,
        playerStats: window.playerStats
    },

    created() {
        this.position.sport = null;
    },

    computed: {
        defaultFeaturedStats() {
            if (this.position.featuredStatIds === null) {
                return [];
            }

            var featuresdStatIds = this.position.featuredStatIds.split(',').map(s => parseInt(s.trim()));
            return this.playerStats.filter(ps => featuresdStatIds.includes(ps.sportPlayerStatId));
        }
    },

    methods: {
        setFeaturedStatIds(featuredStats) {
            this.position.featuredStatIds = featuredStats.map(s => s.sportPlayerStatId).join(',');
        },

        updateSportPosition(position) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/siteapi/sports/" + self.sportId + "/position/" + position.sportPositionId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(position),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.$bvToast.toast('You have updated ' + position.name + ' featured stats.', {
                            title: 'Updated Featured Stats',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                    } else {
                        self.$bvToast.toast('An error occurred', {
                            title: 'Save Position Error',
                            appendToast: true,
                            noAutoHide: true,
                            solid: true,
                            variant: "danger"
                        });
                    }
                });
        }
    }
});