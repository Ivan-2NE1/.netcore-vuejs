<template>
    <vue-selectbase v-bind:input-id="inputId"
                    v-bind:label="label !== null && label !== '' ? label : enableMultiple ? 'States' : 'State'"
                    choose-prompt="Select State"
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
        name: "state-list",
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
            inputId: {
                type: String,
                default: "",
            },
            defaultValue: {
                type: [Array, String, Number, Object],
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
            label: {
                type: String,
                default: ""
            }
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                options: [],
                selected: [],
                validated: false,
                cLoading: false,

            }
        },

        created() {
            var vm = this;
            // load the list from the API endpoint
            $.get("/siteapi/states/list", function (data) {
                vm.options = data;

                if (vm.defaultValue !== undefined && vm.defaultValue !== null && vm.defaultValue !== "") {
                    if (Array.isArray(vm.defaultValue)) {
                        // default value is an array
                        vm.selected = [];
                        for (var i = 0; i < vm.defaultValue.length; i++) {
                            var selId = vm.defaultValue[i];
                            // get the selected value and make it active
                            var selOpt = data.find(a => {
                                return a.id === selId;
                            });
                            if (selOpt !== null) {
                                vm.selected.push(selOpt);
                            }
                        }
                    }
                    else {
                        // default value is a string
                        var selOpt = data.find(a => {
                            return a.id === vm.defaultValue;
                        });

                        if (selOpt !== null) {
                            vm.selected.push(selOpt);
                        }
                    }
                }
            });
        },

        watch: {
            selected(newval) {
                this.$emit('change', newval);
            }
        },
    };
</script>