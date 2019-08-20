<template>
    <vue-selectbase v-bind:label="label"
                    v-bind:form-help="formHelp"
                    choose-prompt="Select Golf Play Format"
                    v-bind:default-value="selected"
                    v-bind:enable-multiple="false"
                    v-bind:required="required"
                    v-bind:external-invalid="externalInvalid"
                    v-bind:options="options"
                    option-label-key="name"
                    v-on:change="selected = $event"></vue-selectbase>
</template>

<script>
    import vueSelectBase from "../base/vue-selectbase/vue-selectbase.vue";

    export default {
        name: "golf-play-format",
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
            label: {
                type: String,
                default: "Golf Play Format",
            },
            formHelp: {
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
        },
        data() {
            return {
                // A computed property can't be used
                // because `data` is evaluated first.
                options: [
                    { id: 0, name: "Stroke" },
                    { id: 1, name: "Match" }
                ],
                selected: [],
                validated: false,
                cLoading: false,

            }
        },

        created() {
            var vm = this;

            if (vm.defaultValue !== null && vm.defaultValue.length > 0) {
                vm.selected = [];
                for (var i = 0; i < vm.defaultValue.length; i++) {
                    var selId = vm.defaultValue[i];
                    // get the selected value and make it active
                    var selOpt = vm.options.find(function (a) {
                        return a.id === selId;
                    });
                    if (selOpt !== null) {
                        vm.selected.push(selOpt);
                    }
                }
            }
        },

        watch: {
            selected(newval) {
                this.$emit('change', newval);
            }
        },
    };
</script>