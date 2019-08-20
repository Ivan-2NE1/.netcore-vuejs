<template>
    <div v-bind:class="['form-group', {'has-focus': hasFocus, 'input-lg': inputSize === 'lg', 'input-sm': inputSize === 'sm'}]">
        <label :class="['control-label', {'has-value': hasValue}]"
               :for="inputId"
               v-if="label !== null && label !== ''">{{ label }}</label>
        <textarea v-bind:id="inputId === '' ? false : inputId"
                  class="form-control"
                  v-bind:name="inputId"
                  v-bind:placeholder="placeholder !== null && placeholder !== '' ? placeholder : ''"
                  v-bind:maxlength="maxLength > 0 ? maxLength : false"
                  v-bind:required="required"
                  v-model.trim="inputValue"
                  v-on:change="inputChanged"
                  v-ascii
                  v-bind:disabled="inputDisabled"
                  @blur="validated = true; hasFocus = false"
                  @focus.native="onFocus"
                  @focus="onFocus"></textarea>
        <div class="form-text text-muted" v-if="formHelp !== null && formHelp !== ''">{{ formHelp }}</div>
    </div>
</template>

<script>
    export default {
        name: "input-textarea",
        props: {
            inputId: {
                type: String,
                default: () => {
                    return 'input-textarea-' + Math.random().toString(36).substr(2, 9)
                }
            },
            defaultValue: {
                type: String,
                default: ""
            },
            label: {
                type: String,
                default: ""
            },
            placeholder: {
                type: String,
                default: ""
            },
            required: {
                type: Boolean,
                default: false
            },
            formHelp: {
                type: String,
                default: ""
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
            inputDisabled: {
                type: Boolean,
                default: false
            }
        },
        data() {
            return {
                inputValue: this.defaultValue,
                inputType: this.type,
                validated: false,
                hasFocus: false,
            };
        },
        computed: {
            hasValue() {
                return (this.inputValue !== null && this.inputValue !== "");
            },
        },

        watch: {
            inputValue(newval) {
                var emitVal = newval;
                if (this.maxLength !== null && this.maxLength > 0 && newval.length > this.maxLength) {
                    emitVal = emitVal.substring(0, this.maxLength);
                }
                this.$emit("input", emitVal);
            },
            defaultValue(newVal) {
                this.inputValue = newVal;
            }
        },

        directives: {
            ascii: {
                bind(el, binding, vnode) {
                    var handler = function (e) {
                        var oldVal = e.target.value;
                        var newVal = oldVal.replace(/[^\x00-\x7F]/g, '');

                        if (oldVal !== newVal) {
                            // remove all non-ASCII characters and trigger an 'input' event
                            e.target.value = newVal;

                            if (typeof (Event) === 'function') {
                                var event = new Event('input');
                            }
                            else {
                                var event = document.createEvent('Event');
                                event.initEvent('input', true, true);
                            }

                            vnode.elm.dispatchEvent(event);
                        }
                    }

                    el.addEventListener('input', handler);
                }
            }
        },

        methods: {
            inputChanged(newVal) {
                this.$emit("change", this.inputValue);
            },

            onFocus() {
                this.hasFocus = true;
                this.$emit("focus")
            },
        }
    };
</script>
