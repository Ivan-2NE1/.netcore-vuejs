import WidgetWrapper from "../components/base/widget-wrapper/widget-wrapper.vue";
import InputField from "../components/base/input-field/input-field.vue";
import TableGrid from "../components/base/table-grid/table-grid.vue";

var teamDistributionList = new Vue({
    el: '#vueApp',

    components: {
        "widget-wrapper": WidgetWrapper,
        "input-field": InputField,
        "table-grid": TableGrid,
    },

    data: {
        team: window.team,
        distributionlist: [],
        rowColInfo: window.colInfo,
        addSubscriptionForm: {
            emailAddress: "",
        },
    },

    created() {
        var vm = this;

    },

    mounted() {
    },

    computed: {
        sortedDistributionList() {
            var vm = this;
            vm.distributionlist = vm.team.school.notifyList;

            return vm.distributionlist;
        },

        formInvalid() {
            let invalid = false;

            if (this.addSubscriptionForm.emailAddress === null || this.addSubscriptionForm.emailAddress === "") {
                invalid = true;
            }

            return invalid;
        }
    },

    methods: {
        createSubscription() {

        },

        saveDistribution() {

        },

        deleteDistribution() {

        }
    }
});