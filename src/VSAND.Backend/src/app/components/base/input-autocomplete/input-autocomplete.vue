<template>
    <div v-bind:class="['form-group', {'has-error': !valid, 'has-focus': hasFocus, 'input-lg': inputSize === 'lg', 'input-sm': inputSize === 'sm'}]">
        <label :class="['control-label', {'has-value': hasValue}]"
               :for="inputId === '' ? false : inputId" v-if="label !== ''">{{ label }}</label>

        <autocomplete v-bind:source="asyncSrcUrl"
                      :results-display="typeAheadKey"
                      :results-value="typeAheadAsyncItem"
                      input-class="form-control"
                      :placeholder="placeholder"
                      v-bind:initial-value="value ? value.id : null"
                      v-bind:initial-display="value ? value.name : null"
                      clear-button-icon="fas fa-times"
                      v-on:selected="selectedValue = $event.selectedObject">
        </autocomplete>
    </div>
</template>

<script>
    import Autocomplete from '../autocomplete/autocomplete.vue'

    export default {
        name: "input-autocomplete",
        components: {
            Autocomplete
        },
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-autocomplete-' + Math.random().toString(36).substr(2, 9)
                }
            },
            label: {
                type: String,
                default: ""
            },
            placeholder: {
                type: String,
                default: ""
            },
            value: {
                type: Object,
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
            validationRe: {
                type: String,
                default: ""
            },
            invalidMsg: {
                type: String,
                default: ""
            },
            formHelp: {
                type: String,
                default: ""
            },
            inputMask: {
                type: Object,
                default: null
            },
            loading: {
                type: Boolean,
                default: false
            },
            maxLength: {
                type: Number,
                default: null
            },
            inputSize: {
                type: String,
                default: "",
                validator(value) {
                    // The value must match one of these strings
                    return ['', 'sm', 'lg'].indexOf(value) !== -1
                }
            },
            sourceKey: {
                type: String,
                required: true,
            },
            sourceData: {
                type: Array,
                default: null
            },
            asyncSrcUrl: {
                type: String,
                default: ""
            },
            asyncSrcItem: {
                type: String,
                default: ""
            },
            noMatchMsg: {
                type: String,
                default: "No matching results"
            }
        },

        data() {
            return {
                hasFocus: false,
                selectedValue: this.value,
                typeAheadData: this.sourceData,
                typeAheadKey: this.sourceKey,
                typeAheadAsyncUrl: this.asyncSrcUrl,
                typeAheadAsyncItem: this.asyncSrcItem
            }
        },

        computed: {
            valid() {
                var bValidRe = true;
                if (this.validationRegEx !== "") {
                    var oRe = new RegExp(this.validationRegEx);
                    if (!oRe.test(this.inputValue)) {
                        bValidRe = false;
                    }
                }
                if (!this.validated) return true;
                if (this.externalInvalid !== null && this.externalInvalid) {
                    // double-check to see if we have an internal validator we can try
                    if (!bValidRe) {
                        return false;
                    }
                }
                if (!bValidRe) {
                    return false;
                }
                if (!this.required) return true;
                if (this.inputValue === null || this.inputValue === "") return false;
                return true;
            },
            hasValue() {
                return (this.inputValue !== null && this.inputValue !== "");
            },
        },

        watch: {
            selectedValue(newval) {
                this.$emit("update:value", newval);
            }
        }
    };
</script>