SELECT	t.TitleID, 
		i.ItemID,
		p.PageID,
		t.FullTitle + ' ' + t.PartNumber + ' ' + t.PartName AS Title, 
		dbo.fnCOinSAuthorStringForTitle(t.TitleID, 0) AS Authors,
		dbo.fnAssociationStringForTitle(t.TitleID) AS Associations,
		dbo.fnVariantStringForTitle(t.TitleID) AS Variants,
		dbo.fnKeywordStringForTitle(t.TitleID) AS Keywords,
		dbo.fnCollectionStringForTitleAndItem(t.TitleID, i.ItemID) AS Collections,
		t.Datafield_260_a AS PublicationPlace,
		t.Datafield_260_b AS PublisherName,
		t.Datafield_260_c AS PublicationDate,
		t.StartYear,
		t.EndYear,
		i.Year,
		t.EditionStatement,
		l.LanguageName,
		inst.InstitutionName,
		b.BibliographicLevelName,
		i.Volume,
		dbo.fnPageTypeStringForPage(p.PageID) AS PageTypes,
		ISNULL(ip.PagePrefix, '') AS PagePrefix,
		ISNULL(ip.PageNumber, '') AS PageNumber
FROM	dbo.Title t 
		INNER JOIN dbo.TitleItem ti on t.TitleID = ti.TitleID
		INNER JOIN dbo.Item i on ti.ItemID = i.ItemID
		INNER JOIN dbo.Page p on i.ItemID = p.ItemID
		LEFT JOIN dbo.IndicatedPage ip on p.PageID = ip.PageID AND ip.Sequence = 1
		INNER JOIN dbo.Language l ON i.LanguageCode = l.LanguageCode
		INNER JOIN dbo.Institution inst ON i.InstitutionCode = inst.InstitutionCode
		INNER JOIN dbo.BibliographicLevel b ON t.BibliographicLevelID = b.BibliographicLevelID
WHERE	i.Barcode = 'proceedingsofzoo19221zool'
AND		t.PublishReady = 1
AND		i.ItemStatusID = 40
AND		p.Active = 1
