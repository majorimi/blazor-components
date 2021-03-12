export function getHtmlTitle() {
	return document.title;
}
export function setHtmlTitle(newTitle) {
	if (newTitle) {
		document.title = newTitle;
	}
}

export function getAllLinkHeadTags(relType) {
	let links = null;
	if (relType) {
		links = getTagsFromHead('link', tag => {
			if (tag.rel === relType)
				return true;
			return false;
		});
	}
	else {
		links = getTagsFromHead('link');
	}

	if (links && links.length) {
		let result = [];
		links.forEach(link => {
			let obj = {
				Crossorigin: link.crossorigin,
				Href: link.href,
				HrefLang: link.hreflang,
				Media: link.media,
				ReferrerPolicy: link.referrerpolicy,
				Rel: link.rel,
				Sizes: link.sizes.toString(),
				Title: link.title,
				Type: link.type
			};

			result.push(obj);
		});

		return result;
	}

	return links;
}

export function setFavIconsHeadTags(favIcons) {
	if (!favIcons && !favIcons.length) {
		return;
	}

	let icons = getTagsFromHead('link', tag => {
		if (tag.rel === 'icon')
			return true;
		return false;
	});

	if (icons && icons.length) {
		icons.forEach(icon => {
			document.head.removeChild(icon);
		});
	}

	favIcons.forEach(icon => {
		let newFavIcon = document.createElement('link');
		newFavIcon.rel = 'icon';
		newFavIcon.href = icon.href;
		newFavIcon.sizes = icon.sizes;

		document.head.appendChild(newFavIcon);
	});
}

function getTagsFromHead(tagName, filter) {
	if (!tagName)
		return null;

	let headTags = document.querySelectorAll('head > *');
	let retTags = [];

	for (let i = 0; i < headTags.length; i++) {
		let tag = headTags[i];
		if (tag.tagName == tagName.toUpperCase()) {
			if (!filter || filter(tag)) {
				retTags.push(tag);
			}
		}
	};

	return retTags;
}
function isTagExistsInHead(tagName, filter) {
	let ret = getTagsFromHead(tagName, filter);

	if (ret && ret.length) {
		return true;
	}

	return false;
}