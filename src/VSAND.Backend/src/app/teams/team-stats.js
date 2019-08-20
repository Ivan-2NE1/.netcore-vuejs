import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";

var teamRoster = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
    },

    data: {
        team: window.team,
        playerStatCategories: null,
        playerStats: null,
        teamStatistics: null,
    },

    watch: {
    },

    created() {
        var vm = this;
        $.get("/SiteApi/Sports/" + vm.team.sportId + "/TeamStatistics", function (data) {
            vm.teamStatistics = data;
        });

        $.get("/SiteApi/Sports/" + vm.team.sportId + "/PlayerStatCategories", function (data) {
            vm.playerStatCategories = data;
            console.log("11111", vm.playerStatCategories);
        });

        $.get("/SiteApi/Sports/" + vm.team.sportId + "/PlayerStats", function (data) {
            vm.playerStats = data;
            console.log("22222", vm.playerStats);
        });
    },

    mounted() {
    },

    computed: {
    },

    methods: {
    }
});