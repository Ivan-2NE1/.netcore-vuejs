<template>
    <vue-selectbase v-bind:input-id="inputId"
                    v-bind:label="enableMultiple ? 'Event Types' : 'Event Type'"
                    choose-prompt="Select an Event Type"
                    v-bind:default-value="selected"
                    v-bind:enable-multiple="enableMultiple"
                    v-bind:required="required"
                    v-bind:external-invalid="externalInvalid"
                    v-bind:options="options"
                    option-label-key="name"
                    v-on:change="selected = $event"></vue-selectbase>
</template>

<script>
    import vueSelectBase from "../base/vue-selectbase/vue-selectbase.vue";

    export default {
        name: "eventtype-list",
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        components: {
            "vue-selectbase": vueSelectBase
        },
        props: {
            sportId: {
                type: Number,
                required: true,
            },
            scheduleYearId: {
                type: Number,
                required: true,
            },
            inputId: {
                type: String,
                default: "",
            },
            defaultValue: {
                type: [String, Array],
                default: null
            },
            required: {
                type: Boolean,
                default: false
            },
            externalInvalid: {
                type: Boolean,
                default: null
            },
            enableMultiple: {
                type: Boolean,
                default: false,
            },
            activeOnly: {
                type: Boolean,
                default: true,
            }
        },
        data: function () {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                selectedSportId: this.sportId,
                selectedScheduleYearId: this.scheduleYearId,
                options: [],
                selected: [],
                validated: false,
                cLoading: false,
            }
        },

        created() {
            var vm = this;

            var url = "/siteapi/eventtypes/list?sportid=" + vm.sportId + "&scheduleYearId=" + vm.scheduleYearId;
            if (!this.activeOnly) {
                url = "/siteapi/eventtypes/listall?sportid=" + vm.sportId + "&scheduleYearId=" + vm.scheduleYearId;
            }

            // load the list from the API endpoint
            $.get(url, function (data) {
                vm.options = data;
                
                if (vm.defaultValue !== null) {
                    if (Array.isArray(vm.defaultValue)) {
                        vm.selected = [];
                        for (var i = 0; i < vm.defaultValue.length; i++) {
                            var selId = vm.defaultValue[i];
                            // get the selected value and make it active
                            var selOpt = data.find(function (a) {
                                return a.id === selId;
                            });
                            if (selOpt !== null) {
                                vm.selected.push(selOpt);
                            }
                        }
                    } else {
                        var selOpt = data.find(function (a) {
                            return a.id === vm.defaultValue;
                        });
                        if (selOpt !== undefined && selOpt !== null) {
                            vm.selected.push(selOpt);                            
                        }
                    }
                }

                if (vm.selected.length === 0 && data !== null && data.length > 0) {
                    // this should always default select the "Regular Season" event type
                    vm.selected.push(data[0]);
                    vm.$emit('change', data[0]);
                }
            });
        },

        watch: {
            selected: function (newval) {
                this.$emit('change', newval);
            }
        },
    };
</script>