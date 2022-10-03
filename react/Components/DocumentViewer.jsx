import React from 'react';
import PropTypes from 'prop-types';
import DocViewer, { DocViewerRenderers } from 'react-doc-viewer';

function DocumentViewer(props) {
    return (
        <DocViewer
            documents={[
                {
                    uri: props.fileUrl,
                },
            ]}
            pluginRenderers={DocViewerRenderers}
            config={{ header: { disableHeader: !props.isFileNameDisplayed } }}
        />
    );
}

DocumentViewer.propTypes = {
    fileUrl: PropTypes.string.isRequired,
    isFileNameDisplayed: PropTypes.bool.isRequired,
};

export default DocumentViewer;
