<template>
    <vue-selectbase v-bind:input-id="inputId"
                    v-bind:label="enableMultiple ? 'Sports' : 'Sport'"
                    choose-prompt="Select Sport"
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
        name: "sport-list",
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
            $.get("/siteapi/sports/list", function (data) {
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
            });
        },

        watch: {
            selected(newval) {
                this.$emit('change', newval);
            }
        },
    };
</script>