import TableGrid from "../../components/base/table-grid/table-grid.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";

var scheduleYearSummary = new Vue({
    el: '#vueApp',
    components: {
        "table-grid": TableGrid,
        "widget-wrapper": WidgetWrapper
    },
    data: {
        scheduleYear: window.scheduleYear,
        rowColInfo: window.colInfo
    },
    created() {
        //var self = this;

        //$.get('/siteapi/scheduleyears/' + scheduleYearId, function (data) {
        //    self.scheduleYear = data;
        //}, 'json');
    },
    computed: {
        //sortedScheduleYears() {
        //    if (this.scheduleYears === undefined || this.scheduleYears === null || this.scheduleYears.length === 0) return [];

        //    var oFirst = this.scheduleYears.find(function (x) {
        //        return x.active === true;
        //    });

        //    var oRest = this.scheduleYears.filter(function (x) {
        //        return x.active !== true;
        //    });

        //    // TODO: This should return the single active schedule year first, then all others by EndYear descending
        //    oRest.sort(function (a, b) {
        //        if (a.endYear > b.endYear) {
        //            return -1;
        //        }
        //        if (a.endYear < b.endYear) {
        //            return 1;
        //        }
        //        return 0;
        //    });

        //    var oRet = [];
        //    oRet.push(oFirst);
        //    oRet.push(...oRest);
        //    return oRet;
        //}
    },
    methods: {
        
    }
});