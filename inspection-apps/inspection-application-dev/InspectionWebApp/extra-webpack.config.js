module.exports = {
  module: {
    rules: [{
      include: [/node_modules\/moddle-xml/],
      sideEffects: true
    }, {
      include: [/node_modules\/diagram-js/],
      sideEffects: true
    }, {
      include: [/node_modules\/bpmn-js/],
      sideEffects: true
    }, {
      include: [/node_modules\/bpmn-moddle/],
      sideEffects: true
    }]
  }
};
