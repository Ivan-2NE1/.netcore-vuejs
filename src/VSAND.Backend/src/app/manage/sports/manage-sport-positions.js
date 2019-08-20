import InputField from "../../components/base/input-field/input-field.vue";
import InputCheckbox from "../../components/base/input-checkbox/input-checkbox.vue";
import InputSelect from "../../components/base/input-select/input-select.vue";
import WidgetWrapper from "../../components/base/widget-wrapper/widget-wrapper.vue";
import SportSidebar from "../../components/navs/sport-sidebar.vue";
import TableGrid from "../../components/base/table-grid/table-grid.vue";

import { ToastPlugin } from "bootstrap-vue";
Vue.use(ToastPlugin);

var sportPositions = new Vue({
    el: '#vueApp',
    components: {
        "input-field": InputField,
        "input-checkbox": InputCheckbox,
        "input-select": InputSelect,
        "widget-wrapper": WidgetWrapper,
        "sport-sidebar": SportSidebar,
        "table-grid": TableGrid
    },
    data: {
        sport: window.sport,
        rowColInfo: window.colInfo,
        addPositionForm: {}
    },
    created() {
        this.addPositionForm = this.getAddPositionForm();
    },
    methods: {
        updateSportPosition(position) {
            var self = this;

            $.ajax({
                method: "PUT",
                url: "/siteapi/sports/" + self.sport.sportId + "/position/" + position.sportPositionId,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(position),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        self.sport.positions = self.sport.positions.map(function (p) {
                            // update the state if the Id matches
                            if (p.sportPositionId === position.sportPositionId) {
                                return position;
                            }

                            // leave it unchanged
                            return p;
                        });

                        self.$bvToast.toast('You have updated ' + position.name, {
                            title: 'Updated Position',
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
        },

        getAddPositionForm() {
            return {
                name: "",
                abbreviation: "",
                sportId: this.sport.sportId
            };
        },

        addPosition() {
            var self = this;
            var position = this.addPositionForm;

            position.sortOrder = this.sport.positions.length + 1;

            $.ajax({
                method: "POST",
                url: "/siteapi/sports/" + self.sport.sportId + "/position",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(position),
                dataType: 'json'
            })
                .done(function (data) {
                    if (data.success === true) {
                        position.sportPositionId = data.id;

                        self.sport.positions.push(position);

                        self.$bvToast.toast('You have added ' + position.name, {
                            title: 'Added Position',
                            autoHideDelay: 5000,
                            appendToast: true,
                            solid: true,
                            variant: "success"
                        });

                        self.addPositionForm = self.getAddPositionForm();
                    } else {
                        self.$bvToast.toast('An error occurred. The position was not created.', {
                            title: 'Add Position Error',
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