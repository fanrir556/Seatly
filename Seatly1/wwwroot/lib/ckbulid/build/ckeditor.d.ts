/**
 * @license Copyright (c) 2014-2024, CKSource Holding sp. z o.o. All rights reserved.
 * For licensing, see LICENSE.md or https://ckeditor.com/legal/ckeditor-oss-license
 */
import { ClassicEditor } from '@ckeditor/ckeditor5-editor-classic';
import { Alignment } from '@ckeditor/ckeditor5-alignment';
import { Bold, Italic, Subscript, Superscript, Underline } from '@ckeditor/ckeditor5-basic-styles';
import type { EditorConfig } from '@ckeditor/ckeditor5-core';
import { Essentials } from '@ckeditor/ckeditor5-essentials';
import { FontBackgroundColor, FontColor, FontFamily, FontSize } from '@ckeditor/ckeditor5-font';
import { GeneralHtmlSupport } from '@ckeditor/ckeditor5-html-support';
import { Image, ImageResize, ImageStyle, ImageToolbar } from '@ckeditor/ckeditor5-image';
import { AutoLink, Link } from '@ckeditor/ckeditor5-link';
import { Paragraph } from '@ckeditor/ckeditor5-paragraph';
import { SourceEditing } from '@ckeditor/ckeditor5-source-editing';
import { SpecialCharacters, SpecialCharactersEssentials } from '@ckeditor/ckeditor5-special-characters';
import { Style } from '@ckeditor/ckeditor5-style';
import { Undo } from '@ckeditor/ckeditor5-undo';
declare class Editor extends ClassicEditor {
    static builtinPlugins: (typeof Alignment | typeof AutoLink | typeof Bold | typeof Essentials | typeof FontBackgroundColor | typeof FontColor | typeof FontFamily | typeof FontSize | typeof GeneralHtmlSupport | typeof Image | typeof ImageResize | typeof ImageStyle | typeof ImageToolbar | typeof Italic | typeof Link | typeof Paragraph | typeof SourceEditing | typeof SpecialCharacters | typeof SpecialCharactersEssentials | typeof Style | typeof Subscript | typeof Superscript | typeof Underline | typeof Undo)[];
    static defaultConfig: EditorConfig;
}
export default Editor;
